//////////// This is my Data Structure Final Works Cited - W3Schools, www.learncs.org, ChatGPT, Powerpoints /////////////////////////
//////////// It crawls the CITS elms website and allows you to use a GUI to search or select on of the pages it discorved through webcrawling. ////////////////////

using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WebCrawlerGUI
{
    public partial class Form1 : Form
    {
        Dictionary<String, String> HashIndex = new Dictionary<String, String>();
        Dictionary<String, int> WordCount = new Dictionary<String, int>();
        String rootSite = "http://citelms.net/Internships/Summer_2018/Fan_Site/";
        Queue<String> LinksQueue1 = new Queue<String>();
        List<String> VisitedLinks = new List<String>();
        WebClient client = new WebClient();
        int max1 = 0;
        string max2 = "";

        string webPage;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Web Crawl Finished \n Please select or search for more information regarding a specific page");
            Debug.WriteLine("Hey this is the console");
            //URLdemo();
            Crawl();
        }

        public void Crawl()
        {
            //CRAWL///////////////////////////////////////////////////////////////////////////////////
            LinksQueue1.Enqueue(rootSite + "index.html");
            while (LinksQueue1.Count() > 0)
            {
                String temp1 = LinksQueue1.Dequeue();
                //Debug.WriteLine("The value of temp1 is " + temp1);
                //Debug.WriteLine(LinksQueue1.Peek());
                if(!(VisitedLinks.Contains(temp1)))
                {
                   
                    webPage = client.DownloadString(temp1);
                    VisitedLinks.Add(temp1);
                    Debug.WriteLine(webPage);
                    
                    string pattern1 = "<title>(.*?)</title>";
                    Match result1 = Regex.Match(webPage, pattern1);
                    string title = "unknown";
                    if (result1.Success)
                    {
                        title = result1.Groups[1].Value;
                        Debug.WriteLine("");
                        Debug.WriteLine("Found title: " + title);
                        HashIndex.Add(title, temp1);
                        listBox1.Items.Add(title);
                    }
                    string pattern2 = "<a href=\"(.*?)\">";
                    MatchCollection result2 = Regex.Matches(webPage, pattern2);
                    foreach (Match m in result2)
                    {
                        String link = m.Groups[1].Value;
                        if (!(link.Contains("http") || link.Contains("void") || link.Contains("pdf")))
                        {
                            Debug.WriteLine(link);
                            LinksQueue1.Enqueue(rootSite + link);
                        }
                    }
                }
            }

            /*
            foreach (string i in LinksQueue1)
            {
                Debug.WriteLine(i);
            } */
            Debug.WriteLine("");
            foreach (string i in HashIndex.Keys)
            {
                Debug.WriteLine(i);
            }
            foreach (string i in HashIndex.Values)
            {
                Debug.WriteLine(i);
            }
        }

        public void WordCounter(String linktemp)                  ///////////////////////////// O(n) It has 3 loops but none of the loops are nested loops so the 3n simplys to O(n)
        {
            max1 = 0;
            max2 = "";
            WordCount.Clear();
            listBox2.Items.Clear();

            string link1 = linktemp;
            webPage = client.DownloadString(link1);
            
            string pattern1 = ">(.*?)<";
            MatchCollection result1 = Regex.Matches(webPage, pattern1);
            StringBuilder stringbuild = new StringBuilder();
            foreach (Match m in result1)
            { 
                //listBox2.Items.Add(m.Groups[1].Value);
                stringbuild.Append(m.Groups[1].Value).Append(" ");
            }

            string page1 = stringbuild.ToString();
            char[] splitter = { ' ', '.',',' };
            string[] words = page1.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            //MessageBox.Show(page1);
           

            foreach (string i in words)
            {
                
                if (WordCount.ContainsKey(i))
                {
                    WordCount[i]++;
                }
                else
                {
                    WordCount.Add(i, 1);
                }
            }

            foreach (string i in WordCount.Keys)
            {
                //Debug.WriteLine(i);  //
                listBox2.Items.Add(i + "      Amount :" + WordCount[i]);
                if(i != "and" && i != "the" && i != "at" && i != "of" && i != "a" && i != "to" && i != "in" && i != "is" && i != "their")
                {
                    if(WordCount[i] > max1)
                    {
                        max1 = WordCount[i];
                        max2 = i;
                    }
                }
            }

            textBox3.Text= ("The most common word is " + max2 + " appearing " + max1 + " times.");

        }

        public void URLdemo()
        {
            
            WebClient client = new WebClient();
            string url1 = "https://www.wikipedia.org";
            string webPage = client.DownloadString(url1);
            Debug.WriteLine(webPage);
            string pattern = "<title>(.*?)</title>";
            // needs using System.Text.RegularExpressions;
            Match result = Regex.Match(webPage, pattern);
            string title = "";
            if (result.Success)
            {
                title = result.Groups[1].Value;
            }
            MessageBox.Show("Found title: " + title);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listbox1 = (ListBox)sender;
            string option1 = listBox1.SelectedItem.ToString();
            int index1 = listBox1.SelectedIndex;
            if (option1 != null)
                WordCounter(HashIndex[option1]);
                MessageBox.Show("Title: " + option1 + "\n\nLink: " + HashIndex[option1] + "\n\n" + "The Most Common Word on this page is \"" + max2 + "\"");
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string option1 = textBox1.Text;
            if(option1 != null)
            {
                if(HashIndex.ContainsKey(option1))
                {
                    WordCounter(HashIndex[option1]);
                    MessageBox.Show("Found \n" + "Title: " + option1 + "\n\nLink: " + HashIndex[option1] + "\n\n" + "The Most Common Word on this page is \"" + max2 + "\"");
                }
                else
                {
                    MessageBox.Show("Error ---- Not Found \nPlease Check Spelling");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //WordCounter();  //
        }
    }
    
}