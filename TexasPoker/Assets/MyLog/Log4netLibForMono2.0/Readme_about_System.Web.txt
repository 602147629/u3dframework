6.	����˵��
MyLogLibrary\Log4netLibForMono2.0����������dll�ļ�������log4net.dll�Ǳ���ģ�System.Web.dll�ǲ��Ǳ���ġ�

6.1.��ҪSystem.Web.dll�ļ���ԭ��
��Ϊlog4net���ԭ����ƴ��ж�.Net Web��Ŀ��֧�֣�������.Net Web��Ŀ�������־��
����log4net�����ʱ����Ҫ�����⼸������֧��

System
System.Configuration
System.Data
System.Web
System.XML

Mono�ٷ���������������������dll��Unity4.X��5.0.0f1�༭��������Mono2.8����ʱ�����Ƕ���dll��ѡ�����ǲ���ȫ�ġ�
��unity4.X,5.X��������������ȫ������unity4.0.0f7������dll,unity4.6.4f1,unity5.0.0f1��û��System.Configuration.dll��System.Web.dll�ļ���
��ʹ��UnityVS�ϵ����ʱ��,Visual Studio����ʾlog4net�ж�System.Web.dll 2.0����������Ҫ�����������ļ������ܵ��ԡ�
��ʱ�����ֱ����Visual Studio�Դ���System.Web.dll 2.0�ļ��ᱨ����Ҫʹ��Mono�ٷ�������System.Web.dll 2.0�ļ�����������System.Web.dll�ļ���Դ��Mono�ٷ���������

6.2.ʹ��˵��
��UnityVS����ʱ����unity3d��Ŀ��ʹ��log4net����ȫû���õ�System.Web���κ����ݣ�ֻ��Ϊ������UnityVS���Ե�Ҫ�����Ե��Ե�ʱ����Ҫ���dll.
����Ϸ��ʽ������ʱ�򡿵�������ʽ������Ϸ����ʱ��������log4net.dll��System.Web.dllȥ�������Ժ�֮�󣬷�����ʱ��ɾ�����dll������һ��������Ϸ�����ļ�����������Ҷ�MyLogHelper.cs�ļ�ͷ�����ĸ���������ѡ�������ѡ���ע�ͣ�����2��3��4�����1��ȥ��������ļ���

6.3ΪʲôҪ������ʱ�����dllɾ������������ע��ѡ���ԭ���ǣ�
Unity��Ȼ��ʹ��C#���ű����ԣ������Դ���Mono 2.8����ʱ��C#��֧���Ƿǳ����޵ġ�.Net���д����ḻ�ĵ�������Ȿ������ʹ�ã�������ΪMono����ʱ֧�������ƣ����磬��׼�ڴ�����ʹ��Unity�ű���ʹ��System.Configuration��System.Web�����ռ��й�һ���ࡣ��ԭ��log4net��System.Web������Asp��־���������System.Configuration(����xml�Ĵ���)��ʹ�ã����Լ�ʹ�ڱ���׶ο��Գ�ȥSystem.Web������(log4net�Ǻ��Ĵ������)�����Ǻ��ѳ�ȥ��System.Configuration����(log4net���Ĵ���ʹ��)������ʽ������Ϸ��Ŀʱ��Unity����dll�������������޷�����ʽ�淢��ʱ������log4net.dll����������Ϸ�������Խ׶ο���ʹ��log4net�Ĺ��ܡ�����ڷ���ʱ������log4net,�����ڴ��뼶���ȥSystem.Configuration��Ӱ�졣


6.4.���⻰
��������ʹ��.Net���Ĺ�����������C#����Unity3d�ű����������������ƣ����Ǵ����������衣��Ϊ����ԭ��Unity4.X-5.X����ʹ��2010�귢����Mono 2.8���ڴ�Unity����δ���ľ�����Ѷ�Mono��������δ����Unity����Ӧ�û����Ŀ�ķ���ƽ̨ʹ��IL2CPP�����ɿ�ƽ̨�Ĵ��룬��������Mono��ƽ̨��ĿǰIL2CPP��Ӧ�ã�unity5.X������WebGL��Ŀ��iOS 64��Ŀ����Դ��http://www.indieace.com/thread-8199-1-1.html

