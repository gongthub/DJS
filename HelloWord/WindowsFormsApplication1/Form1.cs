using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string files = System.IO.Path.GetFullPath("Stores")+"\\HelloWord.dll";
            string assems = "HelloWord.Name" ;

            Assembly asm = Assembly.LoadFrom(files);

            Type type = asm.GetType(assems);

        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            HelloName.GetName name = new HelloName.GetName();
            name.write("1");
            //SinaGP.SinaHelp sina = new SinaGP.SinaHelp();
            //sina.Test();
        }
    }
}
