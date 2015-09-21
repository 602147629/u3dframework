using Client.Base;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Socket.Net
{

    public abstract class BaseGameSocket<T> : Singleton<T> where T : class,new()
    {
        //�������շ���˷������Ļ���Buff
        private byte[] _data_buffer;

        //�����������������￪ʼ��
        private int _data_offset;

        //�����������������ٸ�byte
        private int _data_size;

        //��Ϸ������IP��ַ
        private string _ip;
        private bool _is_read_head;

        //��Ϸ�������˿�
        private int _port;
        //Ҫ����˷��͵����������б�
        private List<PacketOut> _send_list = new List<PacketOut>();
        private System.Net.Sockets.Socket _sock;
        private const int MAX_SEND_QUEUE = 0x3e8;


        //������ݣ�һ�����ڶϿ�����ʱ����� 
        private void Clear()
        {
            this._ip = null;
            this._port = 0;
            this._sock = null;
            List<PacketOut> list = this._send_list;
            lock (list)
            {
                this._send_list.Clear();
            }
            this._is_read_head = false;
            this._data_buffer = null;
            this._data_offset = 0;
            this._data_size = 0;
        }

        //�رտͻ��˵�Socket 
        public void Close()
        {
            try
            {
                System.Net.Sockets.Socket socket = this._sock;
                this.Clear();
                if ((socket != null) && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception exception)
            {
                //Debug.LogError("Connect: " + exception.ToString());
                this.Error(exception.ToString());
            }
        }
        //������Ϸ�����
        public void Connect(string ip, int port)
        {
            try
            {
                this.Close();
                this._ip = ip;
                this._port = port;
                this._sock = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._sock.NoDelay = true;
                this._sock.BeginConnect(this._ip, this._port, new AsyncCallback(this.OnConnect), this._sock);
            }
            catch (Exception exception)
            {
               // Debug.LogError("Connect: " + exception.ToString());
                this.Error(exception.ToString());
            }
        }
        //���Socketû�йرռ����ȷ�������Ϣ���������Ϣ��ʼ�� 
        private void ContinueRead()
        {
            try
            {
                if (this.IsConnected)
                {
                    this._sock.BeginReceive(this._data_buffer, this._data_offset, this._data_size - this._data_offset, SocketFlags.None, new AsyncCallback(this.OnRead), null);
                }
            }
            catch (Exception exception)
            {
                //Debug.LogError(exception.ToString());
                this.Error(exception.ToString());
            }
        }
        //�ӷ��Ͷ��дӲ�������Ϸ�������������� 
        private void ContinueSend()
        {
            PacketOut state = null;
            List<PacketOut> list = this._send_list;
            lock (list)
            {
                if (this._send_list.Count == 0)
                {
                    return;
                }
                state = this._send_list[0];
                if (state.start)
                {
                    return;
                }
                state.start = true;
            }
            System.Net.Sockets.Socket sock = state.sock;
            ByteBuffer bytes = new ByteBuffer(state.buff);
            int len = bytes.ReadInt();
            if (sock.Connected)
            {
                sock.BeginSend(state.buff, 0, state.buff.Length, SocketFlags.None, new AsyncCallback(this.OnSend), state);
            }
        }
        //����ʧ�ܻ������ر�Socketһ����������˷������ر��� 
        protected void Error(string msg)
        {
            this.Close();
            this.OnError(msg);
        }

        ~BaseGameSocket()
        {
            this.Close();
        }
        public int GetSendQueueSize()
        {
            List<PacketOut> list = this._send_list;
            lock (list)
            {
                return this._send_list.Count;
            }
        }
        //�˺���������ȥ���� 
        protected abstract void OnConnect();

        //����ǵ�һ���������ˣ�������Ϣͷ
        private void OnConnect(IAsyncResult ret)
        {
            if (ret.AsyncState == this._sock)
            {
                try
                {
                    this._sock.EndConnect(ret);
                    this.ReadHead();
                    this.OnConnect();
                }
                catch (Exception exception)
                {
                    //Debug.LogError(exception.ToString());
                    this.Error(exception.ToString());
                }
            }
        }
        protected abstract void OnError(string msg);
        protected abstract void OnPack(byte[] data);

        //�з������Ϣ������ʼ�������ֽ�����Ϣͷ�ٽ�����Ϣ��
        private void OnRead(IAsyncResult ar)
        {
            try
            {
                if (this.IsConnected)
                {
                    int num = this._sock.EndReceive(ar);
                    this._data_offset += num;
                    if (num <= 0)
                    {

                    }
                    else if (this._data_offset != this._data_size)
                    {
                        this.ContinueRead();
                    }
                    else if (this._is_read_head)
                    {
                        int num2 = ByteBuffer.ToInt(this._data_buffer);
                        this.ReadData(num2);
                    }
                    else
                    {
                        this.OnPack(this._data_buffer);
                        this.ReadHead();
                    }
                }
            }
            catch (Exception exception)
            {
                //Debug.LogError(exception.ToString());
                this.Error(exception.ToString());
            }
        }
        //�������ͳɹ�����鷢�Ͷ����Ƿ�����Ҫ���͵������������������ 
        private void OnSend(IAsyncResult ar)
        {
            PacketOut asyncState = ar.AsyncState as PacketOut;
            System.Net.Sockets.Socket sock = asyncState.sock;
            if (sock.Connected)
            {
                sock.EndSend(ar);
                List<PacketOut> list = this._send_list;
                lock (list)
                {
                    if (this._send_list.Contains(asyncState))
                    {
                        this._send_list.Remove(asyncState);
                    }
                }
                this.ContinueSend();
            }
        }
        //��ȡ��Ϣ�� 
        private void ReadData(int pack_len)
        {
            this._is_read_head = false;
            this._data_size = pack_len;
            this._data_offset = 0;
            this._data_buffer = new byte[this._data_size];
            this.ContinueRead();
        }


        //��ȡ��Ϣͷ 
        private void ReadHead()
        {
            this._is_read_head = true;
            this._data_size = 4;
            this._data_offset = 0;
            this._data_buffer = new byte[this._data_size];
            this.ContinueRead();
        }
        //����ķ�����Ϣ���Ȱ����ݷ������Ͷ��� 
        public void Send(byte[] buff)
        {
            if (this.IsConnected)
            {
                PacketOut item = new PacketOut
                {
                    start = false,
                    buff = buff,
                    sock = this._sock
                };
                int count = 0;
                List<PacketOut> list = this._send_list;
                lock (list)
                {
                    this._send_list.Add(item);
                    count = this._send_list.Count;
                }
                if (count > 0x3e8)
                {

                }
                else
                {
                    this.ContinueSend();
                }
            }
        }
        public bool IsClosed
        {
            get
            {
                return (this._sock == null);
            }
        }
        public bool IsConnected
        {
            get
            {
                return ((this._sock != null) && this._sock.Connected);
            }
        }
        private class PacketOut
        {
            public byte[] buff;
            public System.Net.Sockets.Socket sock;
            public bool start;
        }
    }
}
