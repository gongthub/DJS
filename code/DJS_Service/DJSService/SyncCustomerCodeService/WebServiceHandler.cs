using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.Web.Services.Protocols;

namespace SyncCustomerCodeService
{

    class WebServiceHandler
    {
        /// <summary>
        /// 全动态调用，无需添加web引用(支持SaopHeader)
        /// </summary>
        /// <param name="url">WebServices地址</param>
        /// <param name="methodname">调用的方法</param>
        /// <param name="args">把webservices里需要的参数按顺序放到这个object[]里</param>
        /// <returns></returns>
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            //这里的namespace是需引用的webservices的命名空间，在这里是写死的可以加一个参数从外面传进来。   
            string spacename = "WebService";
            string classname = GetWsClassName(url);
            try
            {
                //1.获取WSDL   
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");//这里指定web service url，一定要以?WSDL结尾

                ServiceDescription sd = ServiceDescription.Read(stream);

                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.ProtocolName = "soap";
                sdi.Style = ServiceDescriptionImportStyle.Client;
                sdi.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                sdi.AddServiceDescription(sd, null, null);

                //2.生成客户端代理类代码  
                CodeNamespace cn = new CodeNamespace(spacename);
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);

                //3.建立C#编译器 
                CSharpCodeProvider csc = new CSharpCodeProvider();
                ICodeCompiler icc = csc.CreateCompiler();

                //4.设定编译参数   
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;

                //5.添加编译条件
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //6.编译程序集
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                //7.检查是否编译成功
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    return null;
                }
                else
                {
                    //8.生成代理实例成功，反射程序集  
                    System.Reflection.Assembly assembly = cr.CompiledAssembly;
                    Type t = assembly.GetType(spacename + "." + classname,true,true);

                    ////9.SOAPHeader 身份验证
                    //FieldInfo[] arry = t.GetFields();
                    //FieldInfo client = null;
                    //object clientkey = null;
                    //if (soapHeader != null)
                    //{
                    //    //Soap头开始 
                    //    client = t.GetField("mySoapHeader" + "Value");
                    //    Type typeClient = assembly.GetType(spacename + "." + soapHeader.ClassName);//获取客户端验证对象  
                    //    clientkey = Activator.CreateInstance(typeClient);//为验证对象赋值
                    //    foreach (KeyValuePair<string, object> property in soapHeader.Properties)
                    //    {
                    //        typeClient.GetField(property.Key).SetValue(clientkey, property.Value);
                    //    }
                    //    //Soap头结束     
                    //}

                    object obj = Activator.CreateInstance(t);//实例类型对象 

                    //if (soapHeader != null)
                    //{
                    //    client.SetValue(obj, clientkey);//设置Soap头 
                    //}
                    //System.Reflection.MethodInfo login = t.GetMethod(loginMethodname);
                    //if (!bool.Parse(login.ToString()))
                    //{
                    //    errMsg = "WebService验证失败，请检查配置密码或用户名是否正确；";
                    //    return null;
                    //}

                    System.Reflection.MethodInfo mi = t.GetMethod(methodname);

                    //10.反射调用方法
                    return mi.Invoke(obj, args);
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 解析WebService类名
        /// </summary>
        /// <param name="wsUrl"></param>
        /// <returns></returns>
        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        /// <summary>  
        /// SOAP头  
        /// </summary>  
        public class SoapHeader
        {
            /// <summary>  
            /// 构造一个SOAP头  
            /// </summary>  
            public SoapHeader()
            {
                this.Properties = new Dictionary<string, object>();
            }

            /// <summary>  
            /// 构造一个SOAP头  
            /// </summary>  
            /// <param name="className">SOAP头的类名</param>  
            public SoapHeader(string className)
            {
                this.ClassName = className;
                this.Properties = new Dictionary<string, object>();
            }

            /// <summary>  
            /// 构造一个SOAP头  
            /// </summary>  
            /// <param name="className">SOAP头的类名</param>  
            /// <param name="properties">SOAP头的类属性名及属性值</param>  
            public SoapHeader(string className, Dictionary<string, object> properties)
            {
                this.ClassName = className;
                this.Properties = properties;
            }

            /// <summary>  
            /// SOAP头的类名  
            /// </summary>  
            public string ClassName { get; set; }

            /// <summary>  
            /// SOAP头的类属性名及属性值  
            /// </summary>  
            public Dictionary<string, object> Properties { get; set; }

            /// <summary>  
            /// 为SOAP头增加一个属性及值  
            /// </summary>  
            /// <param name="name">SOAP头的类属性名</param>  
            /// <param name="value">SOAP头的类属性值</param>  
            public void AddProperty(string name, object value)
            {
                if (this.Properties == null)
                {
                    this.Properties = new Dictionary<string, object>();
                }
                Properties.Add(name, value);
            }
        }
    }

}
