using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        List<RGBPixel> result;

        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
             txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
             txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
             result= QuantizationProject.NumberOfDistinctColor(ImageMatrix);
            textBox1.Text = QuantizationProject.CountOfNode.ToString();
           
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double START = System.Environment.TickCount;
            int K_text;
            List<EdgeValue> Nodes_List = QuantizationProject.MinumimSpanningAlgorithm(result);
            textBox2.Text = QuantizationProject.SumOfcost.ToString();
         
            if (textBox3.Text.Length == 0)
            {
                K_text = QuantizationProject.Generate_ClstureAtuo(Nodes_List);
                List<List<RGBPixel>> Clusterss = QuantizationProject.Clustring_Group(K_text, result, Nodes_List);
                RGBPixel[,,] valuee = QuantizationProject.NewImageColor(Clusterss);
                RGBPixel[,] QuintizeImagee = QuantizationProject.QuantizationOfImage(valuee, ImageMatrix);
                ImageOperations.DisplayImage(QuintizeImagee, pictureBox2);
                double END1 = System.Environment.TickCount;
                double SUB1 = END1 - START;
                textBox4.Text = (SUB1/ 1000).ToString()+"   Second";
                MessageBox.Show("Number of Atuo cluster  : " + K_text);
            }
            else
            {
                 int my_clusterNum = Convert.ToInt32(textBox3.Text);

                if (my_clusterNum <= QuantizationProject.CountOfNode&&my_clusterNum!=0)
                {
                    List<List<RGBPixel>> Clusters = QuantizationProject.Clustring_Group(my_clusterNum, result, Nodes_List);
                    RGBPixel[,,] value = QuantizationProject.NewImageColor(Clusters);
                    RGBPixel[,] QuintizeImage = QuantizationProject.QuantizationOfImage(value, ImageMatrix);
                    ImageOperations.DisplayImage(QuintizeImage, pictureBox2);
                }
                else if(my_clusterNum==0)
                    MessageBox.Show("Error Num must be greater than 0");
                else
                    MessageBox.Show("Error Num Cluster must be less than or Equal Num Of Distinct color"); 
            double END2 = System.Environment.TickCount;
            double SUB2 = END2 - START;
            textBox4.Text = (SUB2 / 1000).ToString() + "   Second";
            }
          
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void nudMaskSize_ValueChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}