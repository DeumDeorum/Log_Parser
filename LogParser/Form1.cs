using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace LogParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<String> filters = new List<String>();
        int priority = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            //try
            //{
                filters = new List<string>();
                if (textBox2.Text != "") filters.Add(textBox2.Text);
                if (textBox3.Text != "") filters.Add(textBox3.Text);
                if (textBox4.Text != "") filters.Add(textBox4.Text);
                if (textBox5.Text != "") filters.Add(textBox5.Text);
                priority = filters.Count;
                if (ScienceBox.Checked)
                {
                    if (ReferenceBox.Checked)
                    {
                        filters.Add("Scien");
                        filters.Add("RD");
                        filters.Add("Research");
                        filters.Add("director");
                    }
                    foreach (Person p in listOfPeople)
                    {
                        if (p.Job.Contains("Scien") || p.Job.Contains("Director") || p.Job.Contains("Robo"))
                        {
                            filters.Add(p.Name);
                        }
                    }
                }
                if (HeadsBox3.Checked)
                {
                    foreach (Person p in listOfPeople)
                    {
                        if (p.Job.Contains("Head of Security") || p.Job.Contains("Chief") || p.Job.Contains("Direct") || p.Job.Contains("Captain"))
                        {
                            filters.Add(p.Name);
                        }
                    }
                }
                if (SupplyBox.Checked)
                {
                    foreach (Person p in listOfPeople)
                    {
                        if (p.Job.Contains("Cargo") || p.Job.Contains("Quarter"))
                        {
                            filters.Add(p.Name);
                        }
                    }
                }
                if (SecurityBox2.Checked)
                {
                    if (ReferenceBox.Checked)
                    {
                        filters.Add("Security");
                    }
                    foreach (Person p in listOfPeople)
                    {
                        if (p.Job.Contains("Security") || p.Job.Contains("Warden"))
                        {
                            filters.Add(p.Name);
                        }
                    }
                }
                if (CaptainCheckbox.Checked)
                {

                    filters.Add("Captain");
                    if (ReferenceBox.Checked)
                    {
                        filters.Add("Armory");
                    }
                    foreach (Person p in listOfPeople)
                    {
                        if (p.Job.Contains("Captain"))
                        {
                            filters.Add(p.Name);
                        }
                    }
                }
                if (EngineeringBox2.Checked)
                {
                    if (ReferenceBox.Checked)
                    {
                        filters.Add("Atmos");
                        filters.Add("Engine");
                    }
                    foreach (Person p in listOfPeople)
                    {
                        if (p.Job.Contains("Engin") || p.Job.Contains("Atmos") || p.Job.Contains("Signal"))
                        {
                            filters.Add(p.Name);
                        }
                    }
                }
            if (MedicalBox5.Checked)
            {
                if (ReferenceBox.Checked)
                {
                    filters.Add("Med");
                }
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Docto") || p.Job.Contains("Geneti") || p.Job.Contains("Viro"))
                    {
                        filters.Add(p.Name);
                    }
                }
            }
            if (SiliconcheckBox2.Checked)
                {
                    if (ReferenceBox.Checked)
                    {
                        filters.Add("ai ");
                        filters.Add(" borg ");
                        filters.Add("AI ");
                        filters.Add("Ai ");
                    }
                    foreach (Person p in listOfPeople)
                    {
                        if (p.Job.Contains("Cyborg") || p.Job.Contains("AI"))
                        {
                            filters.Add(p.Name);
                        }
                    }
                }
                filterOut = new List<string>();
                if (AttackBox.Checked)
                {
                    filterOut.Add("ATTACK:");
                }
                if (OOCBox.Checked)
                {
                    filterOut.Add("OOC:");
                }
                if (EmoteBox.Checked || MiscBox.Checked)
                {
                    filterOut.Add("EMOTE:");
                }
                if (AccessBox.Checked || MiscBox.Checked)
                {
                    filterOut.Add("ACCESS:");
                }
                if (ByondBox.Checked || MiscBox.Checked)
                {
                    filterOut.Add("BYOND:");
                }
                if (GameBox.Checked || MiscBox.Checked)
                {
                    filterOut.Add("GAME:");
                }
                if (SayBox.Checked)
                {
                    filterOut.Add("SAY:");
                }
                if (AccessBox.Checked || MiscBox.Checked)
                {
                    filterOut.Add("ADMIN:");
                }

                startFilter();
            //}
            //catch (Exception) { };


        }
        
        String[] gameLog = new string[0];
        private void openGameLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog gameDialog = new OpenFileDialog();
            
            if(gameDialog.ShowDialog() == DialogResult.OK)
            {
                gameLog = File.ReadAllLines(gameDialog.FileName);
                   //+= t + "\n";
            }
            
            
           // }
        }
        String[] actionLog = new string[0];
        List<String> filterOut = new List<String>();
        private void startFilter()
        {

            List<DateTime> times = new List<DateTime>();
            richTextBox1.Hide();
            String[] gameLog2 = gameLog.Clone() as String[];
            String[] actionLog2 = actionLog.Clone() as String[];
            if (gameLog2 != null||!(gameLog2.Length<0))
            {

                for (int i = 0; i < gameLog2.Length; i++)
                {
                    Boolean result = false;
                    Boolean filterOutResult = false;
                    Person foundPerson = new Person();
                    String log = gameLog2[i];
                    for (int b = 0; b < filters.Count && !result; b++)
                    {
                        result = log.Contains(filters[b]);

                    }
                    for (int c = 0; c < filterOut.Count && result && !filterOutResult; c++)
                    {
                        filterOutResult = log.Contains(filterOut[c]);
                    }
                    bool personFound = false;

                    if (log.Contains(")"))
                    {
                        foreach (Person p in listOfPeople)
                        {
                            personFound = log.Substring(0, log.IndexOf(")")).Contains(p.Name);
                            foundPerson = p;
                            if (personFound) break;
                        }
                        if (!personFound)
                        {
                            foundPerson.Name = "N00000000000N";
                        }
                        int start = log.IndexOf(")") + 1;
                        int end = log.Length - start;
                        log = log.Substring(start);
                        Char[] reverse = log.ToCharArray();
                        Array.Reverse(reverse);
                        String logReverse = new string(reverse);
                        if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(")+1);
                        if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(")+1);
                        reverse = logReverse.ToCharArray();
                        Array.Reverse(reverse);
                        log = new string(reverse);

                        //if (log.Contains("(")) log = log.Substring(0, log.IndexOf("(") - 1);
                        if (gameLog2[i].Contains("OOC"))
                        {
                            log = "<font color=\"purple\"\"\\>" + log + "</font>";
                            log = "OOC: " + foundPerson.Name + " <font size=\"1\">" + foundPerson.Job + "</font>" + log;
                        }
                        if (gameLog2[i].Contains("ATTACK"))
                        {
                            log = "<font color=\"red\"\"\\>" + log + "</font>";
                            log = "Attack: " + foundPerson.Name + " <font size=\"1\">" + foundPerson.Job + "</font>" + log;
                        }
                        if (gameLog2[i].Contains("SAY"))
                        {
                            log = "<font color=\"GREEN\"\"\\>" + log + "</font>";
                            log = "Says: " + foundPerson.Name + " <font size=\"1\">" + foundPerson.Job + "</font>" + log;
                        }


                        log = "<div class=\"tooltip\">" + log + "<span class=\"tooltiptext\">" /*+ foundPerson.Job + " " + foundPerson.antag*/ + "</span> </div>";


                    }
                    String tempLog = gameLog2[i];
                    DateTime temp = new DateTime();
                    if (tempLog.Contains("[") && tempLog.Contains("]"))
                    {
                        int startTime = tempLog.IndexOf("[") + 1;
                        int endTime = tempLog.IndexOf("]") - 1 - startTime;
                        tempLog = gameLog[i].Substring(startTime, endTime);

                        temp = Convert.ToDateTime(tempLog);
                        times.Add(temp);
                    }
                    else
                    {
                        times.Add(new DateTime(0));
                    }
                    if (ShowTimesBox.Checked) log = "<font size=\"2\">" + temp.ToShortTimeString() + "</font> " + log;
                    if (!result || filterOutResult || foundPerson.Name.Contains("N00000000000N"))
                        log = "";

                    gameLog2[i] = log;
                    

                }
                //   foreach (String t in gameLog)
                //  {ff
                // richTextBox1.Text = String.Join("\n\n", gameLog2.Where((s => !String.IsNullOrEmpty(s))));


            }
            if (actionLog2 != null || !(actionLog2.Length < 0))
            {
                for (int i = 0; i < actionLog2.Length; i++)
                {
                    Boolean result = false;
                    Boolean filterOutResult = false;
                    Person foundPerson = new Person();
                    String log = actionLog2[i];
                    for (int b = 0; b < filters.Count && !result; b++)
                    {
                        result = log.Contains(filters[b]);

                    }
                    for (int c = 0; c < filterOut.Count && result && !filterOutResult; c++)
                    {
                        filterOutResult = log.Contains(filterOut[c]);
                    }
                    bool personFound = false;

                    if (log.Contains(")"))
                    {
                        foreach (Person p in listOfPeople)
                        {
                            personFound = log.Substring(0, log.IndexOf(")")).Contains(p.Name);
                            foundPerson = p;
                            if (personFound) break;
                        }
                        if (!personFound)
                        {
                            foundPerson.Name = "N00000000000N";
                        }
                        int start = log.IndexOf(")") + 1;
                        int end = log.Length - start;
                        log = log.Substring(start);


                        String deeperDive = log.Substring(log.IndexOf("(") +1);

                       
                        Char[] reverse = log.ToCharArray();
                        Array.Reverse(reverse);
                        String logReverse = new string(reverse);
                        if(logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(")+1);
                        if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(")+1);
                        reverse = logReverse.ToCharArray();
                        Array.Reverse(reverse);
                        log = new string(reverse);



                        //if (log.Contains("(")) log = log.Substring(0, log.IndexOf("(") - 1);
                        
                        if (actionLog2[i].Contains("OOC"))
                        {
                            log = "<font color=\"purple\"\"\\>" + log + "</font>";
                            log = "OOC: " + foundPerson.Name + " <font size=\"1\">" + foundPerson.Job + "</font>" + log;
                        }
                        if (actionLog2[i].Contains("ATTACK"))
                        {
                            log = "<font color=\"red\"\"\\>" + log + "</font>";
                            log = "Attack: " + foundPerson.Name + " <font size=\"1\">" + foundPerson.Job + "</font>" + log;
                        }
                        if (actionLog2[i].Contains("SAY"))
                        {
                            log = "<font color=\"GREEN\"\"\\>" + log + "</font>";
                            log = "Says: " + foundPerson.Name + " <font size=\"1\">"+foundPerson.Job+"</font>" + log;
                        }



                        log = "<div class=\"tooltip\">" + log+"<span class=\"tooltiptext\">" +foundPerson.Job+" " + foundPerson.antag + "</span> </div>";
                    }
                    String tempLog = actionLog2[i];
                    DateTime temp = new DateTime();
                    if (tempLog.Contains("[") && tempLog.Contains("]"))
                    {
                        int startTime = tempLog.IndexOf("[") + 1;
                        int endTime = tempLog.IndexOf("]") - 1 - startTime;
                        tempLog = actionLog[i].Substring(startTime, endTime);

                        temp = Convert.ToDateTime(tempLog);
                        times.Add(temp);
                    }
                    else
                    {
                        times.Add(new DateTime(0));
                    }
                    if (ShowTimesBox.Checked) log = "<font size=\"2\">" +temp.ToShortTimeString() + "</font> " + log;
                    if (!result || filterOutResult || foundPerson.Name.Contains("N00000000000N"))
                        log = "";
                    actionLog2[i] = log;
                    

                }
                //   foreach (String t in gameLog)
                //  {ff
                // richTextBox1.Text = String.Join("\n\n", gameLog2.Where((s => !String.IsNullOrEmpty(s))));


            }
            String[] master = new string[gameLog2.Length + actionLog2.Length];
            int f = 0;
            for(int a = 0; a < gameLog.Length; a++)
            {
                Boolean added = false;
                while ( f < actionLog.Length)
                {
                    if(times[a] < times[f+gameLog.Length])
                    {
                        master[a + f] = gameLog2[a];
                        added = true;
                        break;
                    }
                    else
                    {
                        master[a + f] = actionLog2[f];
                        f++;
                        added = true;
                    }
                    
                }
                if (!added)
                {
                    master[a + f] = gameLog2[a];
                }
            }
            for (; f < actionLog.Length; f++)
            {
                master[f + gameLog.Length] = actionLog2[f];
            }
            if (master.Length>0)
                webBrowser1.DocumentText = "<html><style>.tooltip {  position: relative;        display: inline-block;                  " +
                    "}.tooltip.tooltiptext {  visibility: hidden;  width: 120px;  background-color: #555; color: #fff;  text-align: center;" +
                    "  border-radius: 6px;  padding: 5px 0; position: absolute;  z-index: 1;  bottom: 125%;  left: 50%;  margin-left: -60px;" +
                    "  opacity: 0;  transition: opacity 0.3s;}.tooltip.tooltiptext::after { content: \"\";  position: absolute; top: 100%;" +
                    " left: 50%; margin-left: -5px; border-width: 5px; border-style: solid;  border-color: #555 transparent transparent transparent;}" +
                    ".tooltip:hover.tooltiptext { visibility: visible;  opacity: 1;}</style>"+String.Join("", master.Where((s =>handleWhere(s)))) +"<html>";

            //richTextBox1.Show();
        }
        private Boolean handleWhere(String s)
        {
            Boolean result = (!String.IsNullOrEmpty(s) && s.Length > 4);


            return result;
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        List<Person> listOfPeople;
        private void openManifestLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listOfPeople = new List<Person>();
            OpenFileDialog manifestDialog = new OpenFileDialog();

            if (manifestDialog.ShowDialog() == DialogResult.OK)
            {
                String[] manifestLog = File.ReadAllLines(manifestDialog.FileName);
                for(int i = 2; i < manifestLog.Length;i++)
                {
                    if (manifestLog[i].Contains("\\"))
                    {
                        String[] temp = manifestLog[i].Split('\\');
                        Person newPerson = new Person();
                        newPerson.Name = temp[1].Trim(' ');
                        newPerson.Job = temp[2].Trim(' ');
                        newPerson.antag = temp[3].Trim(' ');
                        listOfPeople.Add(newPerson);
                    }

                }

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void openActionLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog actionDialog = new OpenFileDialog();

            if (actionDialog.ShowDialog() == DialogResult.OK)
            {
                actionLog = File.ReadAllLines(actionDialog.FileName);
                //+= t + "\n";
            }
        }
    }
}
