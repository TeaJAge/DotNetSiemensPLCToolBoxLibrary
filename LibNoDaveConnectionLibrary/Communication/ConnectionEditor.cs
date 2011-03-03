﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using DotNetSiemensPLCToolBoxLibrary.General;
using Microsoft.Win32;

namespace DotNetSiemensPLCToolBoxLibrary.Communication
{
    public partial class ConnectionEditor : Form
    {
        public String DefaultConnectionName { get; set; }
        public bool ConnectionNameFixed { get; set; }

        /// <summary>
        /// When this is set, the Configuration is not read or stored to the Registry!
        /// </summary>
        public bool ObjectSavedConfiguration { get; set; }

        public ConnectionEditor()
        {
            InitializeComponent();
        }

        private void ConnectionEditor_Load(object sender, EventArgs e)
        {

            LockControls();

            foreach (string myType in Enum.GetNames(typeof(LibNodaveConnectionTypes)))
                lstLIBNODAVEConnectionType.Items.Add(new EnumListItem(myType, (int)Enum.Parse(typeof(LibNodaveConnectionTypes), myType)));

            foreach (string myType in Enum.GetNames(typeof(LibNodaveConnectionBusSpeed)))
                lstLIBNODAVEBusSpeed.Items.Add(new EnumListItem(myType, (int)Enum.Parse(typeof(LibNodaveConnectionBusSpeed), myType)));

            lstLIBNODAVELokalCOMPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            FillEntryPointsList();

            lstConnectionList.Items.AddRange(PLCConnectionConfiguration.GetConfigurationNames());

            lblConnectionName.Text = DefaultConnectionName;
            lstConnectionList.Text = DefaultConnectionName;

            if (!ConnectionNameFixed && lstConnectionList.Items.Count <= 0)
                lstLIBNODAVEConnectionType.Enabled = false;
            else if (lstConnectionList.Items.Count > 0)
                lstConnectionList.SelectedIndex = 0;

            if (ConnectionNameFixed)
            {
                lstConnectionList.Visible = false;
                cmdConnectionAdd.Visible = false;
                cmdConnectionDelete.Visible = false;
                lblConnectionName.Visible = true;

                LoadSettings();
            }
        }



        private void LockControls()
        {
            lblLIBNODAVEEntryPoint.Enabled = false;
            lstListEntryPoints.Enabled = false;
            lblLIBNODAVECPURack.Enabled = false;
            txtLIBNODAVECPURack.Enabled = false;
            lblLIBNODAVECPUSlot.Enabled = false;
            txtLIBNODAVECPUSlot.Enabled = false;
            lblLIBNODAVECPUMPI.Enabled = false;
            txtLIBNODAVECPUMPI.Enabled = false;
            lblLIBNODAVELokalMPI.Enabled = false;
            txtLIBNODAVELokalMPI.Enabled = false;
            lblLIBNODAVECPUIP.Enabled = false;
            txtLIBNODAVECPUIP.Enabled = false;
            lblLIBNODAVELokalCOMPort.Enabled = false;
            lstLIBNODAVELokalCOMPort.Enabled = false;
            lblLIBNODAVEBusSpeed.Enabled = false;
            lstLIBNODAVEBusSpeed.Enabled = false;
            lblLIBNODAVELokalCOMPort.Enabled = false;
            txtLIBNODAVECPUPort.Enabled = false;
            lblLIBNODAVELokalComSpeed.Enabled = false;
            lstLIBNODAVELokalComSpeed.Enabled = false;
            lblLIBNODAVELokalComParity.Enabled = false;
            lstLIBNODAVELokalComParity.Enabled = false;
            chkNetlinkReset.Enabled = false;
            btnConfigEntryPoint.Enabled = false;
            txtTimeoutIPConnect.Enabled = false;
            lblTimeoutIPConnect.Enabled = false;
            lblTimeoutIPConnectDescr.Enabled = false;
            txtTimeout.Enabled = false;
            lblTimeout.Enabled = false;
            lblTimeoutDescr.Enabled = false;
            lblLIBNODAVECPUPort.Enabled = false;
            txtLIBNODAVECPUMPI.Enabled = false;
            lblLIBNODAVECPUMPI.Enabled = false;

            chkRouting.Enabled = false;
            lblRoutingDestination.Enabled = false;
            lblRoutingMinus.Enabled = false;
            lblRoutingRack.Enabled = false;
            lblRoutingSlot.Enabled = false;
            lblRoutingSubnet.Enabled = false;
            txtRoutingDestination.Enabled = false;
            txtRoutingRack.Enabled = false;
            txtRoutingSlot.Enabled = false;
            txtRoutingSubnetFirst.Enabled = false;
            txtRoutingSubnetSecond.Enabled = false;
        }

        private void LoadSettings()
        {
            String name;
            if (lblConnectionName.Visible)
                name = lblConnectionName.Text;
            else
                name = lstConnectionList.Text;

            //var myConfig = new LibNoDaveConnectionConfiguration(name);
            if (!ObjectSavedConfiguration)
            {
                myConfig = new PLCConnectionConfiguration(name);
            }

            lstListEntryPoints.SelectedItem = myConfig.EntryPoint;

            txtLIBNODAVECPURack.Text = myConfig.CpuRack.ToString();
            txtLIBNODAVECPUSlot.Text = myConfig.CpuSlot.ToString();
            txtLIBNODAVECPUMPI.Text = myConfig.CpuMpi.ToString();
            txtLIBNODAVECPUIP.Text = myConfig.CpuIP;
            txtLIBNODAVECPUPort.Text = myConfig.Port.ToString();
            txtLIBNODAVELokalMPI.Text = myConfig.LokalMpi.ToString();
            lstLIBNODAVELokalCOMPort.SelectedItem = myConfig.ComPort;
            if (myConfig.ComPortParity == 'e')
                lstLIBNODAVELokalComParity.SelectedItem = "even";
            else if (myConfig.ComPortParity == 'o')
                lstLIBNODAVELokalComParity.SelectedItem = "odd";
            else
                lstLIBNODAVELokalComParity.SelectedItem = "none";
            lstLIBNODAVELokalComSpeed.SelectedItem = myConfig.ComPortSpeed;

            EnumListBoxExtensions.SelectEnumListItem(lstLIBNODAVEConnectionType, myConfig.ConnectionType);
            EnumListBoxExtensions.SelectEnumListItem(lstLIBNODAVEBusSpeed, myConfig.BusSpeed);

            txtRoutingDestination.Text = myConfig.RoutingDestination;
            txtRoutingRack.Text = myConfig.RoutingDestinationRack.ToString();
            txtRoutingSlot.Text = myConfig.RoutingDestinationSlot.ToString();
            chkRouting.Checked = myConfig.Routing;
            txtRoutingSubnetFirst.Text = myConfig.RoutingSubnet1.ToString("X");
            txtRoutingSubnetSecond.Text = myConfig.RoutingSubnet2.ToString("X");

            txtTimeout.Text = myConfig.Timeout.ToString();
            txtTimeoutIPConnect.Text = myConfig.TimeoutIPConnect.ToString();

            chkNetlinkReset.Checked = myConfig.NetLinkReset;

            cmdUndo.Visible = false;
            cmdSave.Visible = false;
        }

        public PLCConnectionConfiguration myConfig { get; set; }

        private void updateConfig()
        {
            String name;
            if (lblConnectionName.Visible)
                name = lblConnectionName.Text;
            else
                name = lstConnectionList.Text;

            //var myConfig = new LibNoDaveConnectionConfiguration(name);
            if (myConfig == null)
                myConfig = new PLCConnectionConfiguration(name);

            if (lstListEntryPoints.SelectedItem != null)
                myConfig.EntryPoint = lstListEntryPoints.SelectedItem.ToString();
            myConfig.CpuRack = Convert.ToInt32(txtLIBNODAVECPURack.Text);
            myConfig.CpuSlot = Convert.ToInt32(txtLIBNODAVECPUSlot.Text);
            myConfig.CpuMpi = Convert.ToInt32(txtLIBNODAVECPUMPI.Text);
            myConfig.CpuIP = txtLIBNODAVECPUIP.Text;
            myConfig.Port = Convert.ToInt32(txtLIBNODAVECPUPort.Text);
            myConfig.LokalMpi = Convert.ToInt32(txtLIBNODAVELokalMPI.Text);
            myConfig.ComPort = lstLIBNODAVELokalCOMPort.SelectedItem != null ? lstLIBNODAVELokalCOMPort.SelectedItem.ToString() : "";

            if (lstLIBNODAVELokalComSpeed.SelectedItem != null)
                myConfig.ComPortSpeed = lstLIBNODAVELokalComSpeed.SelectedItem.ToString();

            if (lstLIBNODAVELokalComParity.SelectedItem.ToString() == "even")
                myConfig.ComPortParity = 'e';
            else if (lstLIBNODAVELokalComParity.SelectedItem.ToString() == "odd")
                myConfig.ComPortParity = 'o';
            else
                myConfig.ComPortParity = 'n';

            myConfig.ConnectionType = lstLIBNODAVEConnectionType.SelectedItem != null ? ((EnumListItem)lstLIBNODAVEConnectionType.SelectedItem).Value : 0;

            myConfig.BusSpeed = lstLIBNODAVEBusSpeed.SelectedItem != null ? ((EnumListItem)lstLIBNODAVEBusSpeed.SelectedItem).Value : 0;

            myConfig.NetLinkReset = chkNetlinkReset.Checked;

            myConfig.RoutingDestination = txtRoutingDestination.Text;
            myConfig.RoutingDestinationRack = Convert.ToInt32(txtRoutingRack.Text);
            myConfig.RoutingDestinationSlot = Convert.ToInt32(txtRoutingSlot.Text);
            myConfig.Routing = chkRouting.Checked;
            myConfig.RoutingSubnet1 = Convert.ToInt32(txtRoutingSubnetFirst.Text, 16);
            myConfig.RoutingSubnet2 = Convert.ToInt32(txtRoutingSubnetSecond.Text, 16);

            myConfig.Timeout = Convert.ToInt32(txtTimeout.Text);
            myConfig.TimeoutIPConnect = Convert.ToInt32(txtTimeoutIPConnect.Text);
        }

        private void SaveSettings()
        {
            updateConfig();

            if (!ObjectSavedConfiguration)
                myConfig.SaveConfiguration();

            cmdUndo.Visible = false;
            cmdSave.Visible = false;
        }

        private void lstLIBNODAVEConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LockControls();

            if (lstLIBNODAVEConnectionType.SelectedItem != null)
                switch (((EnumListItem)lstLIBNODAVEConnectionType.SelectedItem).Value)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 10:
                        //lblLIBNODAVEEntryPoint.Enabled = true;
                        //lstListEntryPoints.Enabled = true;
                        lblLIBNODAVECPURack.Enabled = true;
                        txtLIBNODAVECPURack.Enabled = true;
                        lblLIBNODAVECPUSlot.Enabled = true;
                        txtLIBNODAVECPUSlot.Enabled = true;
                        lblLIBNODAVECPUMPI.Enabled = true;
                        txtLIBNODAVECPUMPI.Enabled = true;
                        lblLIBNODAVELokalMPI.Enabled = true;
                        txtLIBNODAVELokalMPI.Enabled = true;
                        lblLIBNODAVELokalCOMPort.Enabled = true;
                        lstLIBNODAVELokalCOMPort.Enabled = true;
                        lblLIBNODAVEBusSpeed.Enabled = true;
                        lstLIBNODAVEBusSpeed.Enabled = true;
                        lblLIBNODAVELokalComSpeed.Enabled = true;
                        lstLIBNODAVELokalComSpeed.Enabled = true;
                        lblLIBNODAVELokalComParity.Enabled = true;
                        lstLIBNODAVELokalComParity.Enabled = true;
                        break;
                    case 20:
                        //lblLIBNODAVEEntryPoint.Enabled = true;
                        //lstListEntryPoints.Enabled = true;
                        //lblLIBNODAVECPURack.Enabled = true;
                        //txtLIBNODAVECPURack.Enabled = true;
                        //lblLIBNODAVECPUSlot.Enabled = true;
                        //txtLIBNODAVECPUSlot.Enabled = true;
                        //lblLIBNODAVECPUMPI.Enabled = true;
                        //txtLIBNODAVECPUMPI.Enabled = true;
                        //lblLIBNODAVELokalMPI.Enabled = true;
                        //txtLIBNODAVELokalMPI.Enabled = true;
                        lblLIBNODAVELokalCOMPort.Enabled = true;
                        lstLIBNODAVELokalCOMPort.Enabled = true;
                        //lblLIBNODAVEBusSpeed.Enabled = true;
                        //lstLIBNODAVEBusSpeed.Enabled = true;
                        //lblLIBNODAVELokalComSpeed.Enabled = true;
                        //lstLIBNODAVELokalComSpeed.Enabled = true;
                        //lblLIBNODAVELokalComParity.Enabled = true;
                        //lstLIBNODAVELokalComParity.Enabled = true;
                        break;
                    case 50:
                        lblLIBNODAVEEntryPoint.Enabled = true;
                        lstListEntryPoints.Enabled = true;
                        btnConfigEntryPoint.Enabled = true;
                        lblLIBNODAVECPUMPI.Enabled = true;
                        txtLIBNODAVECPUMPI.Enabled = true;
                        FillEntryPointsList();
                        EnableDestinationWithEntryPointType();
                        chkRouting.Enabled = true;
                        break;

                    case 122: //ISO TCP
                    case 123: //ISO TCP CP 243
                        lblLIBNODAVECPUIP.Enabled = true;
                        txtLIBNODAVECPUIP.Enabled = true;
                        lblLIBNODAVECPURack.Enabled = true;
                        txtLIBNODAVECPURack.Enabled = true;
                        lblLIBNODAVECPUSlot.Enabled = true;
                        txtLIBNODAVECPUSlot.Enabled = true;
                        lblLIBNODAVECPUPort.Enabled = true;
                        txtLIBNODAVECPUPort.Enabled = true;
                        txtTimeoutIPConnect.Enabled = true;
                        lblTimeoutIPConnect.Enabled = true;
                        lblTimeoutIPConnectDescr.Enabled = true;
                        txtTimeout.Enabled = true;
                        lblTimeout.Enabled = true;
                        lblTimeoutDescr.Enabled = true;
                        txtLIBNODAVECPUPort.Text = "102";
                        chkRouting.Enabled = true;
                        break;

                    case 223:
                    case 224:
                        chkNetlinkReset.Enabled = true;
                        lblLIBNODAVECPUIP.Enabled = true;
                        txtLIBNODAVECPUIP.Enabled = true;
                        lblLIBNODAVECPUMPI.Enabled = true;
                        txtLIBNODAVECPUMPI.Enabled = true;
                        lblLIBNODAVECPURack.Enabled = true;
                        txtLIBNODAVECPURack.Enabled = true;
                        lblLIBNODAVECPUSlot.Enabled = true;
                        txtLIBNODAVECPUSlot.Enabled = true;
                        lblLIBNODAVECPUPort.Enabled = true;
                        txtLIBNODAVECPUPort.Enabled = true;
                        lblLIBNODAVELokalMPI.Enabled = true;
                        txtLIBNODAVELokalMPI.Enabled = true;
                        txtTimeoutIPConnect.Enabled = true;
                        lblTimeoutIPConnect.Enabled = true;
                        lblTimeoutIPConnectDescr.Enabled = true;
                        txtTimeout.Enabled = true;
                        lblTimeout.Enabled = true;
                        lblTimeoutDescr.Enabled = true;
                        txtLIBNODAVECPUPort.Text = "1099";
                        chkRouting.Enabled = true;
                        break;

                    case 230:
                        lblLIBNODAVECPUIP.Enabled = true;
                        txtLIBNODAVECPUIP.Enabled = true;
                        lblLIBNODAVECPUMPI.Enabled = true;
                        txtLIBNODAVECPUMPI.Enabled = true;
                        lblLIBNODAVECPURack.Enabled = true;
                        txtLIBNODAVECPURack.Enabled = true;
                        lblLIBNODAVECPUSlot.Enabled = true;
                        txtLIBNODAVECPUSlot.Enabled = true;
                        lblLIBNODAVECPUPort.Enabled = true;
                        txtLIBNODAVECPUPort.Enabled = true;
                        lblLIBNODAVELokalMPI.Enabled = true;
                        txtLIBNODAVELokalMPI.Enabled = true;
                        txtTimeoutIPConnect.Enabled = true;
                        lblTimeoutIPConnect.Enabled = true;
                        lblTimeoutIPConnectDescr.Enabled = true;
                        txtTimeout.Enabled = true;
                        lblTimeout.Enabled = true;
                        lblTimeoutDescr.Enabled = true;
                        txtLIBNODAVECPUPort.Text = "7777";
                        chkRouting.Enabled = true;
                        break;
                }

            cmdSave.Visible = true;
            cmdUndo.Visible = true;
        }



        private void ValueChanged(object sender, EventArgs e)
        {
            cmdSave.Visible = true;
            cmdUndo.Visible = true;

            if (chkRouting.Checked)
            {
                lblRoutingDestination.Enabled = true;
                lblRoutingMinus.Enabled = true;
                lblRoutingRack.Enabled = true;
                lblRoutingSlot.Enabled = true;
                lblRoutingSubnet.Enabled = true;
                txtRoutingDestination.Enabled = true;
                txtRoutingRack.Enabled = true;
                txtRoutingSlot.Enabled = true;
                txtRoutingSubnetFirst.Enabled = true;
                txtRoutingSubnetSecond.Enabled = true;
            }
            else
            {
                lblRoutingDestination.Enabled = false;
                lblRoutingMinus.Enabled = false;
                lblRoutingRack.Enabled = false;
                lblRoutingSlot.Enabled = false;
                lblRoutingSubnet.Enabled = false;
                txtRoutingDestination.Enabled = false;
                txtRoutingRack.Enabled = false;
                txtRoutingSlot.Enabled = false;
                txtRoutingSubnetFirst.Enabled = false;
                txtRoutingSubnetSecond.Enabled = false;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (cmdSave.Visible)
                if (MessageBox.Show("Nicht gespeichert! Zurück zum Editieren?", "Achtung:", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    return;

            this.Close();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void cmdUndo_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void lstConnectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void btnConfigEntryPoint_Click(object sender, EventArgs e)
        {
            Process myProc = new Process();
            myProc.StartInfo.FileName = "rundll32.exe";
            myProc.StartInfo.Arguments = "shell32.dll, Control_RunDLL S7EPATDX.CPL "; // +lstListEntryPoints.SelectedItem.ToString();
            myProc.Start();
            myProc.WaitForExit();
            FillEntryPointsList();
        }

        public void FillEntryPointsList()
        {
            object selItem = lstListEntryPoints.SelectedItem;

            lstListEntryPoints.Items.Clear();
            RegistryKey myConnectionKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Siemens\\SINEC\\LogNames");
            if (myConnectionKey != null) lstListEntryPoints.Items.AddRange(myConnectionKey.GetSubKeyNames());


            lstListEntryPoints.SelectedItem = selItem;

            EnumListItem tmp = (EnumListItem)lstLIBNODAVEConnectionType.SelectedItem;
            LibNodaveConnectionTypes connTp = 0;
            if (tmp != null) connTp = (LibNodaveConnectionTypes)tmp.Value;
            if (lstConnectionList.SelectedIndex >= 0 && connTp == LibNodaveConnectionTypes.Use_Step7_DLL)
                EnableDestinationWithEntryPointType();
        }

        public void EnableDestinationWithEntryPointType()
        {
            lblS7OnlineDevice.Text = "(none)";

            string akItem = "";

            if (lstListEntryPoints.SelectedItem != null)
                akItem = lstListEntryPoints.SelectedItem.ToString();

            RegistryKey myConnectionKey =
                Registry.LocalMachine.CreateSubKey("SOFTWARE\\Siemens\\SINEC\\LogNames\\" + akItem
                                                   );
            string tmpDevice = (string)myConnectionKey.GetValue("LogDevice");

            string retVal = "";
            if (tmpDevice != "")
            {
                lblS7OnlineDevice.Text = tmpDevice;
                myConnectionKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Siemens\\SINEC\\LogDevices\\" + tmpDevice);
                retVal = (string)myConnectionKey.GetValue("L4_PROTOCOL");
            }

            if (retVal == "TCPIP" || retVal == "ISO")
            {
                txtLIBNODAVECPUIP.Enabled = true;
                txtLIBNODAVECPURack.Enabled = true;
                txtLIBNODAVECPUSlot.Enabled = true;
                txtLIBNODAVECPUMPI.Enabled = false;
                lblLIBNODAVECPUIP.Enabled = true;
                lblLIBNODAVECPURack.Enabled = true;
                lblLIBNODAVECPUSlot.Enabled = true;
                lblLIBNODAVECPUMPI.Enabled = false;
            }
            else
            {
                txtLIBNODAVECPUIP.Enabled = false;
                txtLIBNODAVECPURack.Enabled = false;
                txtLIBNODAVECPUSlot.Enabled = false;
                txtLIBNODAVECPUMPI.Enabled = true;
                lblLIBNODAVECPUIP.Enabled = false;
                lblLIBNODAVECPURack.Enabled = false;
                lblLIBNODAVECPUSlot.Enabled = false;
                lblLIBNODAVECPUMPI.Enabled = true;
            }


        }


        private void lblRoutingSlot_Click(object sender, EventArgs e)
        {

        }

        private void lstListEntryPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValueChanged(sender, e);
            EnableDestinationWithEntryPointType();
        }

        private void lblLIBNODAVELokalCOMPort_Click(object sender, EventArgs e)
        {

        }

        private void cmdConnectionAdd_Click(object sender, EventArgs e)
        {
            string cfgName = "";
            DialogResult ret = InputBox.Show("Enter Name...", "Enter the Name of the new Connection:", ref cfgName);

            if (ret == DialogResult.OK)
            {
                if (cfgName != "")
                {
                    PLCConnectionConfiguration tmp = new PLCConnectionConfiguration(cfgName);
                    tmp.SaveConfiguration();

                    lstConnectionList.Items.Clear();
                    lstConnectionList.Items.AddRange(PLCConnectionConfiguration.GetConfigurationNames());
                    lstConnectionList.SelectedItem = cfgName;
                    lstLIBNODAVEConnectionType.Enabled = true;

                }
            }
        }

        private void lblConnectionName_Click(object sender, EventArgs e)
        {

        }

        private void cmdConnectionDelete_Click(object sender, EventArgs e)
        {
            if (lstConnectionList.SelectedItem != null)
            {
                cmdSave.Visible = false;
                cmdUndo.Visible = false;

                System.Windows.Forms.DialogResult res = MessageBox.Show("Are you sure that you want to delete the Connection : " + lstConnectionList.SelectedItem.ToString(), "Delete realy?", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    LockControls();

                    PLCConnectionConfiguration.DeleteConfiguration(lstConnectionList.SelectedItem.ToString());
                    lstConnectionList.Items.Clear();
                    lstConnectionList.Items.AddRange(PLCConnectionConfiguration.GetConfigurationNames());
                    if (lstConnectionList.Items.Count > 0)
                    {
                        lstConnectionList.SelectedItem = lstConnectionList.Items[0];
                        lstLIBNODAVEConnectionType_SelectedIndexChanged(sender, e);
                    }
                    else
                        lstLIBNODAVEConnectionType.Enabled = false;
                }
            }
        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            cmdTest.Text = "Verbinde...";
            updateConfig();            
            cmdTest.Enabled = false;

            tryConnect.RunWorkerAsync();
        }



        delegate void del(string txt);
        private void changeStatusLabel(string txt)
        {
            if (lblState.InvokeRequired)
            {
                del d = new del(changeStatusLabel);
                this.Invoke(d, new object[] { txt });
                return;
            }
            lblState.Text = txt;
        }

        public void enableCmdTest()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(enableCmdTest));
                return;
            }
            cmdTest.Enabled = true;
        }

        private void tryConnect_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var backup = myConfig.ConfigurationType;
                myConfig.ConfigurationType = LibNodaveConnectionConfigurationType.ObjectSavedConfiguration;
                var tmp = new PLCConnection(myConfig);
                myConfig.ConfigurationType = backup;

                tmp.Connect();
                tmp.Dispose();
                changeStatusLabel("Verbindung erfolgreich!");
            }
            catch (Exception ex)
            {
                changeStatusLabel(ex.Message);
            }
            finally
            {
                enableCmdTest();
            }
        }
    }
}