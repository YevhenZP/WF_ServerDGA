using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormServerDGA.dbServerDGADataSet;
using static WindowsFormServerDGA.Transformers1DataSet;

namespace WindowsFormServerDGA
{
    public partial class MainForm : Form
    {
        TcpListener listener = null;
        string path = String.Empty;

        SqlConnection conn = null;

        DGAResult dgaRes = null;                         //Объявляем объект класса DGAResult и инициализируем его null
        List<DGAResult> results = new List<DGAResult>(); //Объявляем список объектов lbooks типизированный классом DGAResult

        double ch4 = 0, c2h4 = 0, c2h2 = 0;

        public MainForm()
        {
            //string cs = ConfigurationManager.ConnectionStrings["WindowsFormServerDGA.Properties.Settings.dbServerDGAConnectionString"].ConnectionString;
            string cs = ConfigurationManager.ConnectionStrings["WindowsFormServerDGA.Properties.Settings.Transformers1ConnectionString"].ConnectionString;

            conn = new SqlConnection(cs);
            conn.Open();

            InitializeComponent();

            lvDisolvedGases.Columns.Add("ID", 50);
            lvDisolvedGases.Columns.Add("CH4", 60);
            lvDisolvedGases.Columns.Add("C2H4", 60);
            lvDisolvedGases.Columns.Add("C2H2", 60);


            lvDisolvedGases.View = View.Details;
            lvDisolvedGases.FullRowSelect = true;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            DuvalsTriangle t = new DuvalsTriangle();
            //t.calcCoordinates();
            t.drawZone(this);
            //t.trianleDraw(this);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DuvalsTriangle t = new DuvalsTriangle();
            t.triangleCleare(this);
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            try
            {
                listener = new TcpListener(IPAddress.Parse(txtAddress.Text), Convert.ToInt32(txtPort.Text));
                Thread t = new Thread(ListenStart);
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server message");
            }
        }

        private void ListenStart()
        {
            listener.Start();
            ConnectToClient();
        }
        private void ConnectToClient()
        {
            listener.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), listener);
        }

        private void ClientConnected(IAsyncResult ar)
        {
            TcpListener l = ar.AsyncState as TcpListener;
            l.BeginAcceptTcpClient(ClientConnected, l);

            try
            {
                TcpClient client = listener.EndAcceptTcpClient(ar);
                ReciveFile(client);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Server message");
            }
        }
        private void ReciveFile(object obj)
        {
            TcpClient client = obj as TcpClient;
            NetworkStream ns = client.GetStream();

            //string tag = DateTime.Now.ToString();
            //tag = tag.Replace(":", "_");

            //BinaryFormatter formatBinary = new BinaryFormatter();
            try
            {
                //    List<Image> imgList = (List<Image>)formatBinary.Deserialize(ns);
                //    int c = imgList.Count;

                //    ns.Close();
                //    client.Close();

                //    //клиентская машинa - название папки
                //    string dirName = path + tag + "\\";

                //    if (!Directory.Exists(dirName))
                //    {
                //        Directory.CreateDirectory(dirName);
                //    }

                //    for (int i = 0; i < imgList.Count; i++)
                //    {
                //        string file_Name = dirName + tag + "_" + i.ToString() + ".jpg";

                //        //сохраняем картинку
                //        try
                //        {
                //            //BeginInvoke((MethodInvoker)(() => { imgList[i].Save(file_Name, ImageFormat.Jpeg); }));
                //            imgList[i].Save(file_Name, ImageFormat.Jpeg);
                //            Thread.Sleep(10);

                //        }
                //        catch (Exception ex)
                //        {
                //            MessageBox.Show(ex.Message);
                //        }

                //    }
                //    lstFiles.Invoke((MethodInvoker)(() => lstFiles.Items.Add(dirName)));
                //    FillListBox(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Resize(object sender, EventArgs e)
        {
            DuvalsTriangle t = new DuvalsTriangle();
            t.triangleCleare(this);
        }

        private void transformersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.transformersBindingSource1.EndEdit();
                //this.tableAdapterManager.UpdateAll(this.dbServerDGADataSet);
                this.tableAdapterManager1.UpdateAll(this.transformers1DataSet);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "transformers1DataSet.DGAResults". При необходимости она может быть перемещена или удалена.
            this.dGAResultsTableAdapter1.Fill(this.transformers1DataSet.DGAResults);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "transformers1DataSet.Transformers". При необходимости она может быть перемещена или удалена.
            this.transformersTableAdapter1.Fill(this.transformers1DataSet.Transformers);
            
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbServerDGADataSet.DGAResults". При необходимости она может быть перемещена или удалена.
            //this.dGAResultsTableAdapter.Fill(this.dbServerDGADataSet.DGAResults);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dbServerDGADataSet.Transformers". При необходимости она может быть перемещена или удалена.
            //this.transformersTableAdapter.Fill(this.dbServerDGADataSet.Transformers);

            lvDisolvedGasesLoad();
        }

        private void lvDisolvedGasesLoad()
        {
            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select * from DGAResults";

                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    //Создаем объект типа DGAResult
                    dgaRes = new DGAResult();

                    //Инициализиурем свойства обекта данными из таблицы Books базы данны 
                    //из reader, после выполнения комманды "select * from Books"
                    dgaRes.id = reader.GetInt32(reader.GetOrdinal("id"));
                    dgaRes.CH4 = reader.GetDecimal(reader.GetOrdinal("CH4"));
                    dgaRes.C2H4 = reader.GetDecimal(reader.GetOrdinal("C2H4"));
                    dgaRes.C2H2 = reader.GetDecimal(reader.GetOrdinal("C2H2"));


                    //Заполняем список объектов класса DGAResult
                    results.Add(dgaRes);

                }
                reader.Close();

                //Заносим значения в listView1
                int i = 0;
                foreach (var item in results)
                {
                    lvDisolvedGases.Items.Add((item.id).ToString());
                    #region NonCorrect
                    //listView1.Items[item.ID - 1].SubItems.Add((item.AuthorID).ToString());
                    //listView1.Items[item.ID - 1].SubItems.Add((item.Title));
                    //listView1.Items[item.ID - 1].SubItems.Add((item.Price).ToString());
                    //listView1.Items[item.ID - 1].SubItems.Add((item.Pages).ToString());
                    #endregion
                    lvDisolvedGases.Items[i].SubItems.Add((item.id).ToString());
                    lvDisolvedGases.Items[i].SubItems.Add((item.CH4).ToString()); ;
                    lvDisolvedGases.Items[i].SubItems.Add((item.C2H4).ToString());
                    lvDisolvedGases.Items[i].SubItems.Add((item.C2H2).ToString());
                    i += 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void lvDisolvedGases_MouseDown(object sender, MouseEventArgs e)
        //{
        //    //if (e.SelectedItem != null)
        //    //    selected.Text = e.SelectedItem.ToString();
        //        MessageBox.Show("OK "+ (sender as ListView).SelectedItems);
        //}

        private void lvDisolvedGases_SelectedIndexChanged(object sender, EventArgs e)
        {
            //decimal ch4 = (sender as DGAResultsRow).CH4;
            //decimal c2h4 = (sender as DGAResultsRow).C2H4;
            //decimal c2h2 = (sender as DGAResultsRow).C2H2;
        }

        private void dGAResultsDataGridView1_Click(object sender, EventArgs e)
        {
            string txt_ch4 = dGAResultsDataGridView1.CurrentRow.Cells[6].Value.ToString();
            string txt_c2h4 = dGAResultsDataGridView1.CurrentRow.Cells[8].Value.ToString();
            string txt_c2h2 = dGAResultsDataGridView1.CurrentRow.Cells[9].Value.ToString();
            //MessageBox.Show($"ch4 = {txt_ch4}, c2h4 =  {txt_c2h4}, c2h2 =  {txt_c2h2}");

            ch4 = double.Parse(txt_ch4);
            c2h4 = double.Parse(txt_c2h4);
            c2h2 = double.Parse(txt_c2h2);

            DuvalsTriangle t = new DuvalsTriangle();
            t.getLocation(ch4, c2h4, c2h2);
            t.drawZone(this);

        }
    }
}
