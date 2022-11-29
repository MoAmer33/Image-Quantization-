using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class QuantizationProject
    {
        //V EQUAL D EQUAL Distinct color
        const double DIFF = 0.0001;
        public static int CountOfNode;
        public static double SumOfcost = 0;
        static double Avarage;
        static double standerd_div;
        //This function get the distinct color for image 
        public static List<RGBPixel> NumberOfDistinctColor(RGBPixel[,] MatrixOfImage)
        {   
            int WidthOfImage, HeigthOfImage;//->O(1)
            int Red, Green, Blue;//->O(1)
            CountOfNode = 0;//->O(1)   
            HeigthOfImage = ImageOperations.GetHeight(MatrixOfImage);//->O(1)
            WidthOfImage = ImageOperations.GetWidth(MatrixOfImage);//->O(1)
            List<RGBPixel> Vertices = new List<RGBPixel>();//->O(1)  space ->O(D)

            bool[,,] CheckExistColor = new bool[256, 256, 256];//->O(1)
            //This for loop make iteration for all matrix
            for (int row = 0; row < HeigthOfImage; row++) //->O(row) * O(col)   ---->O(N^2)  (Width*Heigth)
            {
                for (int column = 0; column < WidthOfImage; column++)//->O(N)*O(1)     ---->O(N)
                {
                    //This is Value for each pixel
                    Red = MatrixOfImage[row, column].red;//->O(1)
                    Green = MatrixOfImage[row, column].green;//->O(1)
                    Blue = MatrixOfImage[row, column].blue;//->O(1)
                    if (!CheckExistColor[Red,Green,Blue])//->O(1)
                    {
                        //Add the Distict color in new List 
                        Vertices.Add(MatrixOfImage[row, column]);//->O(1)
                        CheckExistColor[Red,Green,Blue] = true;//->O(1)
                    }
                }
            }
            CountOfNode = Vertices.Count;//->O(N)

            return Vertices;//->O(1)
        }
        //Calculate the minumim distance between Nodes
        public static List<EdgeValue> MinumimSpanningAlgorithm(List<RGBPixel> Nodes)
        {
            int count = 0;//->O(1)
            int Root = 0;//->O(1)
            int NextRoot;//->O(1)
            SumOfcost = 0;//->O(1)
            double MinumumCost;//->O(1)
            double[] CostBetwwenNodes = new double[CountOfNode];//->O(1)
            bool[] CheckNode = new bool[CountOfNode];//->O(1)
            int[] PrimaryClass = new int[CountOfNode];//->O(1)
            //Put initial values for this arrays
            while (count < CountOfNode)//->O(V)
            {
                PrimaryClass[count] = -1;//->O(1)
                CostBetwwenNodes[count] = Double.PositiveInfinity;//->O(1)
                count++;//->O(1)
            }
            //Loop for all nodes in the graph 
            for (int Node = 1; Node < CountOfNode; Node++)//->O(V)*O(V)  //-->O(V^2) less than (E log(V))
            {
                CheckNode[Root] = true;//->O(1)
                MinumumCost = Double.PositiveInfinity;//->O(1)
                NextRoot = -1;//->O(1)
                //we initial the Adj by value 1 because the root equal zero and is checked already
                for (int Adj = 1; Adj < CountOfNode; Adj++)//->O(V)
                {
                    //if This node not checked
                    if (CheckNode[Adj] == false)//->O(1)
                    {
                        //This function calculate the distance between Nodes 
                        double distance = EcludienDistance(Nodes, Root, Adj);//->O(1)
                        //Put the value of distance 
                        if (distance < CostBetwwenNodes[Adj])//->O(1)
                        {
                            PrimaryClass[Adj] = Root;//->O(1)
                            CostBetwwenNodes[Adj] = distance;//->O(1)
                        }
                        //check the minumum cost and put it in variable MinumumCost
                        if (CostBetwwenNodes[Adj] < MinumumCost)//->O(1)
                        {
                            NextRoot = Adj;//->O(1)
                            MinumumCost = CostBetwwenNodes[Adj];//->O(1)
                        }
                    }
                    else
                    {
                        continue;//->O(1)
                    }
                }
                SumOfcost += MinumumCost;//->O(1)
                Root = NextRoot;//->O(1)
            }

            List<EdgeValue> MinumumSpanning = new List<EdgeValue>(); //->O(1)
            //i initialized by 1 because we use index zero as root 
            for (int i = 1; i < CountOfNode; i++)//->O(V)
            {
                MinumumSpanning.Add(new EdgeValue(CostBetwwenNodes[i], PrimaryClass[i], i)); //->O(1)
            }
            double var = SumOfcost / (CountOfNode - 1);//->O(1)
            bool[] value = new bool[CountOfNode];//->O(1)
            //This function Calculate Standard Devition
            StandardDiv(MinumumSpanning, value, var,CountOfNode-1); //->O(V)
            return MinumumSpanning;//->O(1)
        }
        //calculate Auto Clusture
        public static int Generate_ClstureAtuo(List<EdgeValue> ListOFMinumum)
        {
            double Avarge_mean = Avarage;//->O(1)
            int Num = CountOfNode - 1;//->O(1)
            bool[] Valid = new bool[Num];//->O(1)
            double new_standard_div = standerd_div;//->O(1)
            double current_standard_div = 0.0;//->O(1)
            int Num_of_cluster = 0;//->O(1)
            //Check if the absolute difference greater than or equal Diff
            while (Math.Abs(current_standard_div - new_standard_div) >= DIFF)//->O(D)   //->O(D)*O(D)   O(D^2)   
            {
                int Index_of_edge = -1;//->O(1)
                double max_value = 0.0;//->O(1)
                double value;//->O(1)
                int count = 0;//->O(1)
                //calculate standerd deviation without divide into CountofNode
                for (int i = 0; i < CountOfNode - 1; i++)//->O(D)
                {
                    //Check the Nodes Checked or not
                    if (Valid[i] == false)//->O(1)
                    {
                        value = (ListOFMinumum[i].Distance - Avarge_mean) * (ListOFMinumum[i].Distance - Avarge_mean);//->O(1)
                       //Check the value greater than Maximum value
                        if (value > max_value)//->O(1)
                        {
                            max_value = value;//->O(1)
                            Index_of_edge = i;//->O(1)
                        }
                    }
                }
                //Check if No Edge
                if (Index_of_edge != -1)//->O(1)
                {
                    Valid[Index_of_edge] = true;//->O(1)
                    Num--;//->O(1)
                    Num_of_cluster++;//->O(1)

                    Avarge_mean = 0.0;//->O(1)
                    current_standard_div = new_standard_div;//->O(1)
                    //Loop for all CountOfNodes
                    while (count < CountOfNode - 1)//->O(D)
                    {
                       if (Valid[count] == false)//->O(1)
                            Avarge_mean = Avarge_mean + ListOFMinumum[count].Distance;//->O(1)
                        count++;//->O(1)
                    }
                    Avarge_mean /= Num;//->O(1)
                    new_standard_div = StandardDiv(ListOFMinumum, Valid, Avarge_mean,Num);//->O(V)
                }
                else
                    break;//->O(1)
            }
            return Num_of_cluster + 1;//->O(1)
        }
        //Calculate the distance between each node
        public static double EcludienDistance(List<RGBPixel> Node, int root, int Count)
        {
            //Calculate the Distance with Ecludien Algorithm Between Nodes
            double red, green, blue;//->O(1)
            blue = Node[Count].blue - Node[root].blue;//->O(1)
            green = Node[Count].green - Node[root].green;//->O(1)
            red = Node[Count].red - Node[root].red;//->O(1)
            return Math.Sqrt((green * green) + (red * red) + (blue * blue));//->O(1)
        }
        //Calculate standerd to use it in Generate_Clsture and Minmum Spanning
        public static double StandardDiv(List<EdgeValue> minumimspan, bool[] Valid, double Count,int Num)
         {
            Avarage = Count;//->O(1)
            double value;//->O(1)
            int i = 0;//->O(1)
            while (i <CountOfNode - 1)//->O(V)
            {
                if (Valid[i] != true)//->O(1)
                {
                    value = minumimspan[i].Distance - Avarage;//->O(1)
                    //calculate standerd deviation 
                    standerd_div = standerd_div + (value * value);//->O(1)
                }
                i++;//->O(1)
            }
            standerd_div = standerd_div / Num;//->O(1)
            standerd_div = Math.Sqrt(standerd_div);//->O(1)
            return standerd_div;//->O(1)
        }
        //Cluster the pixels with distance and generate for cluster for the minumim pixels between them
        public static List<List<RGBPixel>> Clustring_Group(int NumOFCluster, List<RGBPixel> Nodes, List<EdgeValue> edge)
        { 
            int count = 0;//->O(1)
            int cluster = 0;//->O(1)
            double MaximumDistance;//->O(1)
            int index;//->O(1)
            bool[] visted = new bool[CountOfNode];//->O(1)
            List<List<int>> NodeAndAdj = new List<List<int>>();//->O(1)
            List<List<RGBPixel>> Each_Cluster = new List<List<RGBPixel>>();//->O(1)
            //initialize the list by cluster and NodeAndAdj by list 
            while (count<CountOfNode)//->O(D)
            {
                if (count < NumOFCluster)//->O(1)
                    Each_Cluster.Add(new List<RGBPixel>());//->O(1)

                NodeAndAdj.Add(new List<int>());//->O(1)
                count++;//->O(1)
            }
            //we remove the maximum distance by put this value -2 to cluster the graph for the number of cluster we need
            for(int i = 0; i < NumOFCluster-1; i++) //->O(K) ---->//->O(K)*O(D)   --->O(K*D)
            {
                index = -1;//->O(1)
                MaximumDistance = -2;//->O(1)
                for (int j = 0; j < edge.Count; j++)//->O(D)
                { 
                    if (edge[j].Distance > MaximumDistance)//->O(1)
                    {
                        MaximumDistance = edge[j].Distance;//->O(1)
                        index = j;//->O(1) 
                    }  
                }
                edge[index].Distance = -2;//->O(1)
            }
            //we store new node in NodeAndAjc var to make a connected graph 
            for(int i = 0; i < edge.Count; i++)//->O(D)
            {
                if (edge[i].Distance != -2)//->O(1)
                {
                    NodeAndAdj[edge[i].root].Add(edge[i].adjacent);//->O(1)
                    NodeAndAdj[edge[i].adjacent].Add(edge[i].root);//->O(1)
                }
            }
            //Here we use the BFS Algorithm to move of all node to bulid our tree and put it in cluster
            for (int i = 0; i < CountOfNode; i++)//->O(D)     //->O(D) * O(E)
            {
                if (!visted[i])//->O(1)
                {
                    List<int> result = BreadthFirstSearchAlgorithm(i, NodeAndAdj, visted);//->O(D+E)
                    for (int k = 0; k < result.Count; k++)//->O(E)
                    {
                        Each_Cluster[cluster].Add(Nodes[result[k]]);//->O(1)
                    }
                    cluster++;//->O(1)
                }
            }
            return Each_Cluster;//->O(1)
        }
        //BreadthFirstSearchAlgorithm
        public static List<int> BreadthFirstSearchAlgorithm(int root, List<List<int>> adj, bool[] visted)
        {  
            //Check if Node is empty or not
            visted[root] = true; //->O(1)
            Queue<int> queue = new Queue<int>();//->O(1)
            //Add Vertics to Queue
            queue.Enqueue(root);//->O(1)
            List<int> result = new List<int>();//->O(1)
            result.Add(root);//->O(1)
            //Check if Equeue is empty or not
            while (queue.Count != 0)//-->O(D)     -->O(D+E)
            { 
                //Remove the first input in the queue and be root
                int next_root = queue.Dequeue();//->O(1)
                //Loop for Adjacent of root
                foreach (var value in adj[next_root])//->O(E)
                {
                    if (!visted[value])//->O(1)
                    {                      
                        visted[value] = true;//->O(1)
                        queue.Enqueue(value);//->O(1)
                        result.Add(value);//->O(1)
                    }
                }
            }
            return result;//->O(1)
        }
        //Assign all value of Cluster With the same Color and this is average color
        public static RGBPixel[,,]  NewImageColor(List<List<RGBPixel>> ClusterColors)
        {
            //O(E) This edge is elements in cluster
            RGBPixel[,,] NewImage = new RGBPixel[256, 256, 256];//->O(1)
            //Loop for Each cluster and put the value for this cluster with the same value, this value is average
            for (int i = 0; i < ClusterColors.Count; i++)//->O(V)   -->O(V) * O(E)
            {
                RGBPixel result = Average_Of_Each_Cluster(ClusterColors[i],0,0,0);//->O(1)
                //Loop for Element in this cluster
                foreach (var value in ClusterColors[i])//->O(E) 
                {
                    NewImage[value.red, value.green, value.blue] = result;//->O(1)
                }
            }
            return NewImage;//->O(1)
        }
        //Calculate The Centeriod of Each Cluster
        public static RGBPixel Average_Of_Each_Cluster(List<RGBPixel> value,int Green, int Red, int Blue)
        {   
            RGBPixel result;//->O(1)
            //Loop in all elements in this Cluster and sum all values in it
            foreach (var v in value)//->O(D)
            {
                Green =Green+ v.green;//->O(1)
                Red = Red +v.red;//->O(1)
                Blue =Blue+ v.blue;//->O(1)
            }
            //Divide this Sum in Count this Cluster
            Green = Green / value.Count;//->O(1)
            Blue = Blue / value.Count;//->O(1)
            Red = Red / value.Count;//->O(1)
            result = new RGBPixel(Red, Green, Blue);//->O(1)
            return result;//->O(1)
        }
        //Quantize The Image 
        public static RGBPixel[,] QuantizationOfImage(RGBPixel[,,] NewImage,RGBPixel[,] Image)
        {
            //We take the width and height for orginial image 
            int Width = ImageOperations.GetWidth(Image);//->O(1)
            int Height = ImageOperations.GetHeight(Image);//->O(1)
            //We create new array with the width and height 
            RGBPixel[,] QuantizeImage = new RGBPixel[Height,Width];//->O(1)
            //We put the color of index row and column for this image in array NewImage
            //to get the Value that stored in NewImage With the same index
            for (int row = 0; row < Height; row++)//->O(N)    //->O(N) * O(N)     ->O(N^2)    (Row*Column)
                for (int col = 0; col < Width; col++)//->O(N)
                    QuantizeImage[row, col] = NewImage[Image[row, col].red, Image[row, col].green,Image[row, col].blue];//->O(1)

            return QuantizeImage;//->O(1)
        }
    }

}
