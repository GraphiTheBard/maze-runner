// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Reflection.Emit;
public class Program
{



    static void Main(string[] args)
    {
        Bitmap image1;
        Color white = Color.FromArgb(255, 255, 255, 255);



        try
        {
            // Retrieve the image.
            image1 = new Bitmap(@"..\..\mazes\one.png", true);

            int[,] mazeMatrix = new int[image1.Height, image1.Width];

            int x, y;

            Console.WriteLine("height: " + image1.Height + "\n width: " + image1.Width);
            Color newColor = Color.FromArgb(255, 255, 0, 0);
            // Loop through the images pixels to reset color.
            for (x = 0; x < image1.Width; x++)
            {
                for (y = 0; y < image1.Height; y++)
                {
                    Color pixelColor = image1.GetPixel(x, y);


                    if (white.Equals(pixelColor))
                    {
                        image1.SetPixel(1, 1, newColor);
                        mazeMatrix[y, x] = 1;


                    }
                    // Console.Write("  " + mazeGraph[y, x]);


                }
                // 
            }
            image1.Save(@"..\..\solved_mazes\one.png");


            //printing loops, remove later
            for (x = 0; x < image1.Width; x++)
            {
                for (y = 0; y < image1.Height; y++)
                {
                    Console.Write("  " + mazeMatrix[x, y]);

                }
                Console.Write("\n");
            }

            // Set the PictureBox to display the image.
            //  PictureBox1.Image = image1;

            // Display the pixel format in Label1.
            Console.WriteLine("Pixel format: " + image1.PixelFormat.ToString());

            createGraph(image1.Width, image1.Height, mazeMatrix);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("There was an error." +
                    "Check the path to the image file.");
        }
    }

    struct graphNodes
    {
        public int nodeX;
        public int nodeY;
    }


    static void createGraph(int x, int y, int[,] graph)
    {
        int[,] nodeMatrix = new int[x, y];
        int count = 0;
        Console.WriteLine("-------------------------------");



        for (int i = 1; i < y - 1; i++)
        {
            if (graph[0, i] == 1)
            {
                nodeMatrix[0, i] = count;
                Console.WriteLine(count);
                count++;
            }


        }


        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (graph[i, j] == 0)
                    nodeMatrix[i, j] = -2;
            }
        }



        for (int i = 1; i < y - 1; i++)
        {
            for (int j = 1; j < x - 1; j++)
            {
                if (graph[i, j] == 1)
                {
                    if (isANode(i, j, graph))
                    {
                        nodeMatrix[i, j] = count;
                        count++;
                    }
                    else
                        nodeMatrix[i, j] = -1;
                }




            }


        }

        //marking ending node
        for (int i = 1; i < y - 1; i++)
        {
            if (graph[y - 1, i] == 1)
            {
                nodeMatrix[y - 1, i] = count;
                count++;
            }

        }

        graphNodes[] graphNode = new graphNodes[count];

        int structCounter = 0;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (nodeMatrix[i, j] >= 0)
                {

                    graphNode[structCounter].nodeX = i;
                    graphNode[structCounter].nodeY = j;
                    structCounter++;

                }

            }


        }





        int[,] adjMat = new int[count, count];
        int distance, dec, node;

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(graphNode[i].nodeX + " y:" + graphNode[i].nodeY);

            if (graphNode[i].nodeX != 0 && graphNode[i].nodeX != x - 1 && graphNode[i].nodeY != 0 && graphNode[i].nodeY != y - 1)
            {

                //adjacent nodes (distance = 1, for 1 jump )

                if (nodeMatrix[graphNode[i].nodeX - 1, graphNode[i].nodeY] >= 0) //top adjacent
                    adjMat[nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY], nodeMatrix[graphNode[i].nodeX - 1, graphNode[i].nodeY]] = 1;

                if (nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY - 1] >= 0) //left adjacent
                    adjMat[nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY], nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY - 1]] = 1;

                if (nodeMatrix[graphNode[i].nodeX + 1, graphNode[i].nodeY] >= 0) //bottom adjacent
                    adjMat[nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY], nodeMatrix[graphNode[i].nodeX + 1, graphNode[i].nodeY]] = 1;

                if (nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY + 1] >= 0) //right adjacent
                    adjMat[nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY], nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY + 1]] = 1;


                //path adjacent (distance = no.of 0s + 1)
                if (nodeMatrix[graphNode[i].nodeX - 1, graphNode[i].nodeY] == -1)//top adjacent path
                {
                    distance = 1;
                    dec = 1;
                    while (nodeMatrix[graphNode[i].nodeX - dec, graphNode[i].nodeY] == -1)
                    {
                        distance++;
                        dec++;
                    }

                    node = nodeMatrix[graphNode[i].nodeX - dec, graphNode[i].nodeY];

                    if (adjMat[node, nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY]] == 0)
                    {

                        adjMat[nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY], node] = distance;
                    }

                }

                if (nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY - 1] == -1)//left adjacent path
                {
                    distance = 1;
                    dec = 1;
                    while (nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY - dec] == -1)
                    {
                        distance++;
                        dec++;
                    }

                    node = nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY - dec];

                    if (adjMat[node, nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY]] == 0)
                    {
                        adjMat[nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY], node] = distance;
                    }
                }


                if (nodeMatrix[graphNode[i].nodeX + 1, graphNode[i].nodeY] == -1)//bottom adjacent path
                {
                    distance = 1;
                    dec = 1;
                    while (nodeMatrix[graphNode[i].nodeX + dec, graphNode[i].nodeY] == -1)
                    {
                        distance++;
                        dec++;
                        Console.WriteLine(distance + " dec:" + dec);
                    }

                    node = nodeMatrix[graphNode[i].nodeX + dec, graphNode[i].nodeY];

                    if (adjMat[node, nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY]] == 0)
                    {
                        adjMat[nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY], node] = distance;
                    }

                }

                if (nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY + 1] == -1)//right adjacent path
                {
                    distance = 1;
                    dec = 1;
                    while (nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY + dec] == -1)
                    {
                        distance++;
                        dec++;
                    }
                    node = nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY + dec];

                    if (adjMat[node, nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY]] == 0)
                    {
                        adjMat[nodeMatrix[graphNode[i].nodeX, graphNode[i].nodeY], node] = distance;
                    }
                }




            }


        }



        //printing loops, remove later
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Console.Write(nodeMatrix[i, j] + "\t");

            }
            Console.Write("\n");

        }

        Console.Write(count + "     ");


        Console.WriteLine("-------------------------------");
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                Console.Write(adjMat[i, j] + " ");

            }
            Console.Write("\n");

        }



    }




    static bool isANode(int x, int y, int[,] graph)
    {

        try
        {
            //Console.Write("x:" + x + "y:" + y);
            //corner
            if (graph[x, y - 1] == 0 && graph[x - 1, y] == 0 && graph[x, y + 1] == 1 && graph[x + 1, y] == 1)
                return true;

            else if (graph[x, y + 1] == 0 && graph[x - 1, y] == 0 && graph[x, y - 1] == 1 && graph[x + 1, y] == 1)
                return true;

            else if (graph[x, y - 1] == 0 && graph[x + 1, y] == 0 && graph[x, y + 1] == 1 && graph[x - 1, y] == 1)
                return true;

            else if (graph[x, y + 1] == 0 && graph[x + 1, y] == 0 && graph[x, y - 1] == 1 && graph[x - 1, y] == 1)
                return true;

            //junctions 
            else if (graph[x - 1, y] == 0 && graph[x, y - 1] == 1 && graph[x + 1, y] == 1 && graph[x, y + 1] == 1)
                return true;

            else if (graph[x - 1, y] == 1 && graph[x, y - 1] == 0 && graph[x + 1, y] == 1 && graph[x, y + 1] == 1)
                return true;

            else if (graph[x - 1, y] == 1 && graph[x, y - 1] == 1 && graph[x + 1, y] == 0 && graph[x, y + 1] == 1)
                return true;

            else if (graph[x - 1, y] == 1 && graph[x, y - 1] == 1 && graph[x + 1, y] == 1 && graph[x, y + 1] == 0)
                return true;

            else if (graph[x - 1, y] == 1 && graph[x, y - 1] == 1 && graph[x + 1, y] == 1 && graph[x, y + 1] == 0)
                return true;

            //dead ends

            else if (graph[x - 1, y] == 1 && graph[x, y - 1] == 0 && graph[x + 1, y] == 0 && graph[x, y + 1] == 0)
                return true;

            else if (graph[x - 1, y] == 0 && graph[x, y - 1] == 1 && graph[x + 1, y] == 0 && graph[x, y + 1] == 0)
                return true;

            else if (graph[x - 1, y] == 0 && graph[x, y - 1] == 0 && graph[x + 1, y] == 1 && graph[x, y + 1] == 0)
                return true;

            else if (graph[x - 1, y] == 0 && graph[x, y - 1] == 0 && graph[x + 1, y] == 0 && graph[x, y + 1] == 1)
                return true;
            else
                return false;



        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

}
