﻿using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PMPRoomChangeService.Achieve ac = new PMPRoomChangeService.Achieve();
            //ContractChangeLogService.Achieve ac = new ContractChangeLogService.Achieve();
            //RentLoanSummaryNewService.Achieve ac = new RentLoanSummaryNewService.Achieve();
            //RentLoanService.Achieve ac = new RentLoanService.Achieve();
            ac.ExecuteT("job1");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}