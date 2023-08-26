using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp10
{
    class qLearning
    {
        int[,] reward_table;
        double[,] Q_table;
        int blockx, blocky;
        int statex = 0;
        int statey = 0;
        int targetx, targety;
        Random rand = new Random();
        List<int> states = new List<int>();
        List<int> path = new List<int>();
        List<Panel> pathBlock = new List<Panel>();
        Panel maze_panel = new Panel();
        Form Form1 = new Form();
        public List<int> episodeViaStep = new List<int>(); // episode via step
        public List<int> episodeViaCost = new List<int>(); // episode via cost

        int qEquals(double[,] Q_table, int state)
        {
            int flag = 0;
            for(int i=0;i<Q_table.GetLength(0);i++)
            {
                for(int j=i+1;j< Q_table.GetLength(1); j++)
                {
                    if(Q_table[state,i] != Q_table[state,j])
                    {
                        flag = 1;
                    }
                    
                }
            }
            return flag;
        }
        int qMax(double [,] Q_table, int thisState)
        {
            double max = Q_table[thisState, 0];
            int maxIndex = 0;
            List<int> maxIndexList = new List<int>();
            
            for (int i = 1; i < Q_table.GetLength(1); i++)
            {
                if (max <= Q_table[thisState, i])
                {
                   
                    max = Q_table[thisState, i];
                    
                }
                
            }

            for(int i=0;i<Q_table.GetLength(1);i++)
            {
                if(max == Q_table[thisState,i])
                {
                    maxIndexList.Add(i);
                }
            }
            foreach(int number in maxIndexList)
            {
                Console.Write(number + ",");
            }
            Random r = new Random();
            int rast = r.Next(0, (maxIndexList.Count));
            maxIndex = maxIndexList[rast];
            
            Console.WriteLine("Max Index: " + maxIndex);
            return maxIndex;
        }
        double max(double [,] Q_table, int thisState)
        {
            double max = Q_table[thisState, 0];

            int flag = qEquals(Q_table, thisState);
            if(flag==0)
            {
                int action = rand.Next(0, 8);
                max = Q_table[thisState, action];
                
            }
            else
            {
                for (int i = 1; i < Q_table.GetLength(1); i++)
                {
                    if (max < Q_table[thisState, i])
                    {
                        max = Q_table[thisState, i];
                    }
                }
                
            }
            return max;
        }
            
        int stateCount(int [,] reward_table, int statex, int statey)
        {
            
            int count = -1;
            for(int i=0;i<reward_table.GetLength(0);i++)
            {
                for(int j=0;j<reward_table.GetLength(1);j++)
                {
                        count++;
                    if(statex==j && statey==i)
                    {
                        return count;
                    }
                       
                }
            }
            return count;

        }
        
        public qLearning(int[,] reward_table, double[,] Q_table, int blockx, int blocky, int targetx, int targety,int startx, int starty, List<Panel> pathBlock, Panel maze_panel)
        {
            this.reward_table = reward_table;
            this.Q_table = Q_table;
            this.blockx = blockx;
            this.blocky = blocky;
            this.pathBlock = pathBlock;
            this.targetx = targetx;
            this.targety = targety;
            this.statex = startx;
            this.statey = starty;
            this.maze_panel = maze_panel;
        }
        public void q_learning()
        {
            foreach(int number in reward_table)
                {
                states.Add(number);
            }
            /*foreach(int number in states)
            {
                Console.Write(number + " , ");
            }*/
            for(int i=0;i<blocky;i++)
            {
                for(int j=0;j<blockx;j++)
                {
                    Console.Write(reward_table[i, j] + "\t");
                }
                Console.WriteLine();
            }
            
           
            int startx = statex;
            int starty = statey;
            int iter = 0;
            int episode = 0;
            int episode1 = 0;
            double yi = 0.9;
            string harita = "";
            Boolean control = true;
            int cost = 0;
            
            while (control==true)
            {
                
                
                int state = stateCount(reward_table, statex, statey);
                foreach (Control item in maze_panel.Controls.OfType<Control>())
                {

                       string p = "System.Windows.Forms.Panel, BorderStyle: System.Windows.Forms.BorderStyle.None";
                    if (item.Location.X == pathBlock[state].Location.X && item.Location.Y == pathBlock[state].Location.Y && item.ToString().Equals(p))
                    {
                        item.BackColor = Color.FromArgb(149, 175, 192);
                        // Console.WriteLine(item);

                    }

                }
               // pathBlock[state].BackColor = Color.Black;
                int flag = qEquals(Q_table, state);
                Console.WriteLine("state: " + state + " eşitmi= " + flag);
                
               if(flag == 0)
                {
                    
                    int action = rand.Next(0, 8);
                    Console.WriteLine("Action: " + action);
                    if(action == 0)
                    {
                        statey--;
                        Console.WriteLine("stateyyy = " + statey);
                        if (statey <0)
                        {
                            statey++;
                            Console.WriteLine("gitmedi");
                            Console.WriteLine("\n");
                            iter++;
                            continue;
                        }
                        else
                        {
                            
                            int nextState = stateCount(reward_table, statex, statey);
                            Q_table[state, action] = states[nextState] + yi * max(Q_table,nextState);
                            Console.WriteLine("statex: " + statex + " statey: " + statey);
                            Console.Write(states[nextState] + "yi= " + yi + "max= " + max(Q_table, nextState));
                            Console.WriteLine("Belmann: " + Q_table[state, action]);
                            if(states[nextState] == -5)
                            {
                                statex = startx;
                                statey = starty;
                                episode++;
                                state = stateCount(reward_table, startx, starty);
                                Console.WriteLine("startx= " + startx + "starty= " + starty + "state= " +state);
                                cost -= 5;
                                iter = 0;
                                episodeViaStep.Add(iter);
                                episodeViaCost.Add(cost);
                                continue;
                            }
                            else
                            {
                                cost += 3;
                                state = nextState;
                                iter++;

                            }
                            

                        }

                    }
                    else if (action == 1)
                    {
                        statey++;
                        Console.WriteLine("stateyyy = " + statey);
                        if (statey > blocky - 1)
                        {
                            statey--;
                           
                            Console.WriteLine("gitmedi");
                            Console.WriteLine("\n");
                            iter++;
                            continue;
                        }
                        else
                        {
                            
                            int nextState = stateCount(reward_table, statex, statey);
                            Q_table[state, action] = states[nextState] + yi * max(Q_table, nextState);
                            Console.WriteLine("statex: " + statex + " statey: " + statey);
                            Console.Write(states[nextState] + "yi= " + yi + "max= " + max(Q_table, nextState));
                            Console.WriteLine("Belmann: " + Q_table[state, action] );

                            
                            if (states[nextState] == -5)
                            {
                                statex = startx;
                                statey = starty;
                                cost -= 5;
                                episode++;
                                state = stateCount(reward_table, startx, starty);
                                Console.WriteLine("startx= " + startx + "starty= " + starty + "state= " + state);
                                iter = 0;
                                episodeViaStep.Add(iter);
                                episodeViaCost.Add(cost);
                                continue;
                            }
                            else
                            {
                                cost += 3;
                                state = nextState;
                                iter++;

                            }
                        }

                    }
                    else if (action == 2)
                    {
                        statex--;
                        Console.WriteLine("statexxxx = " + statex);
                        if (statex < 0)
                        {
                            statex++;
                            Console.WriteLine("gitmedi");
                            Console.WriteLine("\n");
                            iter++;
                            continue;
                        }
                        else
                        {
                            
                            int nextState = stateCount(reward_table, statex, statey);
                            Q_table[state, action] = states[nextState] + yi * max(Q_table, nextState);
                            
                            Console.WriteLine("statex: " + statex + " statey: " + statey);
                            Console.Write(states[nextState] + "yi= " + yi + "max= " + max(Q_table, nextState));
                            Console.WriteLine("Belmann: " + Q_table[state, action]);
                            if (states[nextState] == -5)
                            {
                                statex = startx;
                                statey = starty;
                                cost -= 5;
                                episode++;
                                state = stateCount(reward_table, startx, starty);
                                Console.WriteLine("startx= " + startx + "starty= " + starty + "state= " + state);
                                iter = 0;
                                episodeViaStep.Add(iter);
                                episodeViaCost.Add(cost);
                                continue;
                            }
                            else
                            {
                                cost += 3;
                                state = nextState;
                                
                                iter++;

                            }

                        }

                    }
                    else if (action == 3)
                    {
                        statex++;
                        Console.WriteLine("statexxxx = " + statex);
                        if (statex > blockx - 1)
                        {
                            statex--;
                            Console.WriteLine("gitmedi");
                            Console.WriteLine("\n");
                            iter++;
                            continue;
                        }
                        else
                        {

                            int nextState = stateCount(reward_table, statex, statey);
                            Q_table[state, action] = states[nextState] + yi * max(Q_table, nextState);

                            Console.WriteLine("statex: " + statex + " statey: " + statey);
                            Console.Write(states[nextState] + "yi= " + yi + "max= " + max(Q_table, nextState));
                            Console.WriteLine("Belmann: " + Q_table[state, action]);
                            if (states[nextState] == -5)
                            {
                                statex = startx;
                                statey = starty;
                                episode++;
                                cost -= 5;
                                state = stateCount(reward_table, startx, starty);
                                iter = 0;
                                episodeViaStep.Add(iter);
                                episodeViaCost.Add(cost);
                                Console.WriteLine("startx= " + startx + "starty= " + starty + "state= " + state);
                                continue;
                            }
                            else
                            {
                                cost += 3;
                                state = nextState;

                                iter++;

                            }

                        }

                    }
                   

                }
                else
                {
                    int action = qMax(Q_table, state);
                    Console.WriteLine("Actions: " + action);
                    if (action == 0)
                    {
                        statey--;
                        Console.WriteLine("stateyyy = " + statey);
                        if (statey < 0)
                        {
                            statey++;
                            Console.WriteLine("gitmedi");
                            Console.WriteLine("\n");
                            iter++;
                            continue;
                        }
                        else
                        {
                            
                            int nextState = stateCount(reward_table, statex, statey);
                            Q_table[state, action] = states[nextState] + yi * max(Q_table, nextState);
                            Console.WriteLine("statex: " + statex + " statey: " + statey);
                            Console.Write(states[nextState] + "yi= " + yi + "max= " + max(Q_table, nextState));
                            Console.WriteLine("Belmann: " + Q_table[state, action]);
                            if (states[nextState] == -5)
                            {
                                statex = startx;
                                statey = starty;
                                cost -= 5;
                                episode++;
                                state = stateCount(reward_table, startx, starty);
                                Console.WriteLine("startx= " + startx + "starty= " + starty + "state= " + state);
                                iter = 0;
                                episodeViaStep.Add(iter);
                                episodeViaCost.Add(cost);
                                continue;

                            }
                            else
                            {
                                
                                state = nextState;
                                cost += 3;
                                iter++;

                            }

                        }
                    }
                    else if (action == 1)
                    {
                        statey++;
                        Console.WriteLine("stateyyy = " + state);
                        if (statey > blocky - 1)
                        {
                            statey--;
                            Console.WriteLine("gitmedi");
                            Console.WriteLine("\n");
                            iter++;
                            continue;
                        }
                        else
                        {
                            
                            int nextState = stateCount(reward_table, statex, statey);
                            Q_table[state, action] = states[nextState] + yi * max(Q_table, nextState);
                            
                            Console.WriteLine("statex: " + statex + " statey: " + statey);
                            Console.Write(states[nextState] + "yi= " + yi + "max= " + max(Q_table, nextState));
                            Console.WriteLine("Belmann: " + Q_table[state, action]);
                            if (states[nextState] == -5)
                            {
                                statex = startx;
                                statey = starty;
                                cost -= 5;
                                episode++;
                                state = stateCount(reward_table, startx, starty);
                                Console.WriteLine("startx= " + startx + "starty= " + starty + "state= " + state);
                                iter = 0;
                                episodeViaStep.Add(iter);
                                episodeViaCost.Add(cost);
                                continue;
                            }
                            else
                            {
                                
                                state = nextState;
                                cost += 3;
                                iter++;

                            }

                        }

                    }
                    else if (action == 2)
                    {
                        statex--;
                        Console.WriteLine("statexxxx = " + statex);
                        if (statex < 0)
                        {
                            statex++;
                            Console.WriteLine("gitmedi");
                            Console.WriteLine("\n");
                            iter++;
                            continue;
                        }
                        else
                        {
                            
                            int nextState = stateCount(reward_table, statex, statey);
                            Q_table[state, action] = states[nextState] + yi * max(Q_table, nextState);
                            
                            Console.WriteLine("statex: " + statex + " statey: " + statey);
                            Console.Write(states[nextState] + "yi= " + yi + "max= " + max(Q_table, nextState));
                            Console.WriteLine("Belmann: " + Q_table[state, action]);
                            if (states[nextState] == -5)
                            {
                                statex = startx;
                                statey = starty;
                                cost -= 5;
                                episode++;
                                state = stateCount(reward_table, startx, starty);
                                Console.WriteLine("startx= " + startx + "starty= " + starty + "state= " + state);
                                iter = 0;
                                episodeViaStep.Add(iter);
                                episodeViaCost.Add(cost);
                                continue;
                            }
                            else
                            {
                                
                                state = nextState;
                                cost += 3;
                                iter++;

                            }

                        }

                    }
                    else if (action == 3)
                    {
                        statex++;
                        Console.WriteLine("statexxxx = " + statex);
                        if (statex > blockx-1)
                        {
                            statex--;
                            Console.WriteLine("gitmedi");
                            Console.WriteLine("\n");
                            iter++;
                            continue;
                        }
                        else
                        {
                            
                            int nextState = stateCount(reward_table, statex, statey);
                            Q_table[state, action] = states[nextState] + yi * max(Q_table, nextState);                            
                            Console.WriteLine("statex: " + statex + " statey: " + statey);
                            Console.Write(states[nextState] + "yi= " + yi + "max= " + max(Q_table, nextState));
                            Console.WriteLine("Belmann: " + Q_table[state, action]);
                            if (states[nextState] == -5)
                            {
                                statex = startx;
                                statey = starty;
                                cost -= 5;
                                episode++;
                                state = stateCount(reward_table, startx, starty);
                                iter = 0;
                                episodeViaStep.Add(iter);
                                episodeViaCost.Add(cost);
                                Console.WriteLine("startx= " + startx + "starty= " + starty + "state= " + state);
                                continue;
                            }
                            else
                            {
                                
                                state = nextState;
                                cost += 3;
                                iter++;

                            }

                        }

                    }
                    


                }
                path.Add(state);
                if (state == stateCount(reward_table,targetx,targety))
                {
                    List<int> pathTemp = new List<int>();
                    path.Add(stateCount(reward_table, startx, starty));
                    cost += 5;
                    episode++;
                    episode1++;
                    iter = 0;
                    cost = 0;
                    episodeViaStep.Add(iter);
                    episodeViaCost.Add(cost);
                    if (episode1 == 50)
                    {
                        
                        foreach(int number in path)
                        {
                            foreach (Control item in maze_panel.Controls.OfType<Control>())
                            {

                                string p = "System.Windows.Forms.Panel, BorderStyle: System.Windows.Forms.BorderStyle.None";
                                if (item.Location.X == pathBlock[number].Location.X && item.Location.Y == pathBlock[number].Location.Y && item.ToString().Equals(p))
                                {
                                    item.BackColor = Color.FromArgb(34, 166, 179); 
                                    // Console.WriteLine(item);

                                }

                            }
                        }
                        
                        /*EpisodeViaCost evc = new EpisodeViaCost(episodeViaCost);
                        evc.Visible = true;
                        EpisodeViaStep evs = new EpisodeViaStep(episodeViaStep);
                        evs.Visible = true;*/
                        
                       
                        break;
                    }
                    statex = startx;
                    statey = starty;
                    Console.WriteLine("gitti= " + state);
                    pathBlock[state].BackColor = Color.Black;
                    path.Clear();
                    
                    
                    
                }
                episodeViaStep.Add(iter);
                episodeViaCost.Add(cost);

                foreach (Control item in maze_panel.Controls.OfType<Control>())
                {

                    string p = "System.Windows.Forms.Panel, BorderStyle: System.Windows.Forms.BorderStyle.None";
                    if (item.Location.X == pathBlock[state].Location.X && item.Location.Y == pathBlock[state].Location.Y && item.ToString().Equals(p))
                    {
                        item.BackColor = Color.FromArgb(106, 176, 76);
                        // Console.WriteLine(item);

                    }
                }
                Application.DoEvents();
                
                
                // pause long enough to see it before the next one happens
                System.Threading.Thread.Sleep(45);
                /*}*/
                Console.WriteLine("\n");
                Console.WriteLine("iteration: " + iter);
                
                
                


            }
            for(int i=0;i<episodeViaStep.Count;i++)
            {
               
                    Console.WriteLine("episode: " + i + " step:" +episodeViaStep[i]);
                

            }
            for (int i = 0; i < episodeViaCost.Count; i++)
            {

                Console.WriteLine("episode: " + i + " Cost:" + episodeViaCost[i]);


            }
            for (int i = 0; i < Q_table.GetLength(0); i++)
            {
                for (int j = 0; j < Q_table.GetLength(1); j++)
                {
                    Console.Write(Q_table[i, j] + " ");
                }
                Console.WriteLine();
            }
                foreach (int number in path)
            {
                Console.Write(number + "->");
            }
            for (int i = 0; i < reward_table.GetLength(0); i++)
            {
                for (int j = 0; j < reward_table.GetLength(1); j++)
                {
                    harita += reward_table[i, j].ToString() + "\t";
                }
                harita += "\n";
            }
            File.WriteAllText("engel.txt", harita);

        }


    }
}
