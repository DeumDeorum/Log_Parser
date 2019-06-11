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
using mshtml;
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
        String[] actionLog = new string[0];
        List<String> filterOut = new List<String>();
        List<DateTime> times = new List<DateTime>();
        private void startFilter()
        {
            times = new List<DateTime>();
            richTextBox1.Hide();
            String[] gameLog2 = gameLog.Clone() as String[];
            String[] actionLog2 = actionLog.Clone() as String[];

            if (gameLog2 != null || !(gameLog2.Length < 0)) gameLog2 = cleanLogsAccordingToFilters(gameLog);
            if (actionLog2 != null || !(actionLog2.Length < 0)) actionLog2 = cleanLogsAccordingToFilters(actionLog2);


            //Currently Merge of logs is hardcoded. Possible to allow for more logs to become smashed in. However, both ui should be edited before this is done.
            //Other features are more important at the moment.
            String[] master = new string[gameLog2.Length + actionLog2.Length];
            int actionIndex = 0;
            for (int mergeIndex = 0; mergeIndex < gameLog.Length; mergeIndex++)
            {
                Boolean added = false;
                while (actionIndex < actionLog.Length)
                {
                    if (times[mergeIndex] < times[actionIndex + gameLog.Length])
                    {
                        master[mergeIndex + actionIndex] = gameLog2[mergeIndex];
                        added = true;
                        break;
                    }
                    else
                    {
                        master[mergeIndex + actionIndex] = actionLog2[actionIndex];
                        actionIndex++;
                        added = true;
                    }

                }
                if (!added)
                {
                    master[mergeIndex + actionIndex] = gameLog2[mergeIndex];
                }
            }
            for (; actionIndex < actionLog.Length; actionIndex++)
            {
                master[actionIndex + gameLog.Length] = actionLog2[actionIndex];
            }
            if (master.Length > 0)
            {         
                webBrowser1.DocumentText = disgustingHorseShitCSSYouDontNNeedToSee()
                + String.Join("", master.Where((s => handleWhere(s)))) + "</div ><html>";
            }

            //richTextBox1.Show();
        }
        private String[] cleanLogsAccordingToFilters(String[] tempLog)
        {
            String[] tempLog2 = tempLog.Clone() as String[];
            tempLog.CopyTo(tempLog2,0);
            for (int i = 0; i < tempLog2.Length; i++)
            {
                Boolean result = false;
                Boolean filterOutResult = false;
                Person person = new Person();
                String log = tempLog2[i].ToLower();
                for (int b = 0; b < filters.Count && !result; b++)
                {
                    result = log.Contains(filters[b].ToLower());

                }
                for (int c = 0; c < filterOut.Count && result && !filterOutResult; c++)
                {
                    filterOutResult = log.Contains(filterOut[c].ToLower());
                }
                bool didFindPerson = false;

                if (log.Contains(")"))
                {

                    didFindPerson = ((person = findPerson(log)).Name.Length>0);

                    int start = log.IndexOf(")") + 1;
                    int end = log.Length - start;
                    log = log.Substring(start);

                    log = trimLocationFromLog(log);

//                    string encasePerson = "";
//                   if (didFindPerson)
//                    {
//                        encasePerson = "<Details> <Summary> " + person.Name + "</Summary> Yeah i contain info </Details>";
 //                   }

                    //if (log.Contains("(")) log = log.Substring(0, log.IndexOf("(") - 1);

                    if (tempLog2[i].Contains("OOC"))
                    {
                        log = "<font color=\"purple\"\">" + log + "</font>";
                        log = displayToolTip(log, person, "OOC: ");
                    }
                    if (tempLog2[i].Contains("ATTACK"))
                    {
                        log = "<font color=\"red\"\">" + log + "</font>";
                        log = displayToolTip(log, person, "Attack");
                    }
                    if (tempLog2[i].Contains("SAY"))
                    {
                        log = "<font color=\"GREEN\"\">" + log + "</font>";
                        log = displayToolTip(log, person, "Says: ");
                    }
                }
                parseLogTime(tempLog2[i], tempLog[i]);
                //Removed for now
               // if (ShowTimesBox.Checked) log = "[" + times[times.Count-1].ToLongTimeString() + "]:  " + log;
                if (!result || filterOutResult || person.Name.Contains("N00000000000N"))
                    log = "";
                if (log.Length > 1) log = "<div> <inline> " + log + " </inline> </div> ";
                tempLog2[i] = log;
            }
            return tempLog2;
        }



        //Current search by each persons ckey
        private Person findPerson(String log)
        {
            Person foundPerson = new Person();
            Boolean didFindPerson = false;
            foreach (Person p in listOfPeople)
            {
                didFindPerson = log.Substring(0, log.IndexOf(')')).Contains(p.ckey.ToLower());
                foundPerson = p;
                if (didFindPerson)
                    break;
            }
            if (!didFindPerson)
            {
                foundPerson.Name = "N00000000000N";
            }
            return foundPerson;
        }
        private String trimLocationFromLog(String log)
        {
            Char[] reverse = log.ToCharArray();
            Array.Reverse(reverse);
            String logReverse = new string(reverse);
            if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(") + 1);
            if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(") + 1);
            reverse = logReverse.ToCharArray();
            Array.Reverse(reverse);
            return new string(reverse);
        }

        private void parseLogTime(String copyOfLog,String origin)
        {
            DateTime tempTime = new DateTime();
            if (copyOfLog.Contains("[") && copyOfLog.Contains("]"))
            {
                int startTime = copyOfLog.IndexOf("[") + 1;
                int endTime = copyOfLog.IndexOf("]") - 1 - startTime;
                copyOfLog = origin.Substring(startTime, endTime);
                tempTime = Convert.ToDateTime(copyOfLog);
                times.Add(tempTime);
            }
            else
            {
                times.Add(new DateTime(0));
            }
        }
        //The rest of the code can be ignored, most of it is basic functions that are required but are unlikely to have bugs or other complex issues
        
        //Simple filter and filter out settings. Nothing much to worry about here. Add in here if you want to add filter selection in/out tools.
        private void checkBoxFilters()
        {
            if (textBox2.Text != "")
            {
                checkForNamesMatching(textBox2.Text);
            }
            if (textBox3.Text != "")
            {
                checkForNamesMatching(textBox3.Text);
            }
            if (textBox4.Text != "")
            {
                checkForNamesMatching(textBox4.Text);
            }
            if (textBox5.Text != "")
            {
                checkForNamesMatching(textBox5.Text);
            }
        }
        private void checkForNamesMatching(String text)
        {
            filters.Add(text);
            for (int i = 0; i < listOfPeople.Count; i++)
            {
                if (text.ToLower().Contains(listOfPeople[i].ckey.ToLower()) || text.ToLower().Contains(listOfPeople[i].Name.ToLower()))
                {
                    filters.Add(listOfPeople[i].Name.ToLower());
                    filters.Add(listOfPeople[i].ckey.ToLower());
                }
            }
        }
        private void filterBySelectionTool()
        {
            if (ScienceBox.Checked)
            {
                if (ReferenceBox.Checked)
                {
                    filters.Add("Scien".ToLower());
                    filters.Add("RD".ToLower());
                    filters.Add("Research".ToLower());
                    filters.Add("director".ToLower());
                }
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Scien".ToLower()) || p.Job.Contains("Director".ToLower()) || p.Job.Contains("Robo".ToLower()))
                    {
                        filters.Add(p.Name.ToLower());
                    }
                }
            }
            if (HeadsBox3.Checked)
            {
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Head of Security".ToLower()) || p.Job.Contains("Chief".ToLower()) || p.Job.Contains("Direct".ToLower()) || p.Job.Contains("Captain".ToLower()))
                    {
                        filters.Add(p.Name.ToLower());
                    }
                }
            }
            if (SupplyBox.Checked)
            {
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Cargo".ToLower()) || p.Job.Contains("Quarter".ToLower()))
                    {
                        filters.Add(p.Name.ToLower());
                    }
                }
            }
            if (SecurityBox2.Checked)
            {
                if (ReferenceBox.Checked)
                {
                    filters.Add("Security".ToLower());
                }
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Security".ToLower()) || p.Job.Contains("Warden".ToLower()))
                    {
                        filters.Add(p.Name.ToLower());
                    }
                }
            }
            if (CaptainCheckbox.Checked)
            {

                filters.Add("Captain".ToLower());
                if (ReferenceBox.Checked)
                {
                    filters.Add("Armory".ToLower());
                }
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Captain".ToLower()))
                    {
                        filters.Add(p.Name.ToLower());
                    }
                }
            }
            if (EngineeringBox2.Checked)
            {
                if (ReferenceBox.Checked)
                {
                    filters.Add("Atmos".ToLower());
                    filters.Add("Engine".ToLower());
                }
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Engin".ToLower()) || p.Job.Contains("Atmos".ToLower()) || p.Job.Contains("Signal".ToLower()))
                    {
                        filters.Add(p.Name.ToLower());
                    }
                }
            }
            if (MedicalBox5.Checked)
            {
                if (ReferenceBox.Checked)
                {
                    filters.Add("Med".ToLower());
                }
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Docto".ToLower()) || p.Job.Contains("Geneti".ToLower()) || p.Job.Contains("Viro".ToLower()))
                    {
                        filters.Add(p.Name.ToLower());
                    }
                }
            }
            if (SiliconcheckBox2.Checked)
            {
                if (ReferenceBox.Checked)
                {
                    filters.Add("ai ");
                    filters.Add(" borg ");
                }
                foreach (Person p in listOfPeople)
                {
                    if (p.Job.Contains("Cyborg".ToLower()) || p.Job.Contains("AI".ToLower()))
                    {
                        filters.Add(p.Name.ToLower());
                    }
                }
            }
        }
        private void filterOutBySelectionTool()
        {
            if (AttackBox.Checked)
            {
                filterOut.Add("ATTACK:".ToLower());
            }
            if (OOCBox.Checked)
            {
                filterOut.Add("OOC:".ToLower());
            }
            if (EmoteBox.Checked || MiscBox.Checked)
            {
                filterOut.Add("EMOTE:".ToLower());
            }
            if (AccessBox.Checked || MiscBox.Checked)
            {
                filterOut.Add("ACCESS:".ToLower());
            }
            if (ByondBox.Checked || MiscBox.Checked)
            {
                filterOut.Add("BYOND:".ToLower());
            }
            if (GameBox.Checked || MiscBox.Checked)
            {
                filterOut.Add("GAME:".ToLower());
            }
            if (SayBox.Checked)
            {
                filterOut.Add("SAY:".ToLower());
            }
            if (AccessBox.Checked || MiscBox.Checked)
            {
                filterOut.Add("ADMIN:".ToLower());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //try
            //{
                filters = new List<string>();
                filterOut = new List<string>();

                checkBoxFilters();
                filterBySelectionTool();
                priority = filters.Count;

                filterOutBySelectionTool();

                startFilter();
            //}
            //catch (Exception) { };
        }


        String[] gameLog = new string[0];
        private void openGameLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog gameDialog = new OpenFileDialog();

            if (gameDialog.ShowDialog() == DialogResult.OK)
            {
                gameLog = File.ReadAllLines(gameDialog.FileName);
                //+= t + "\n";
            }


            // }
        }
        private String disgustingHorseShitCSSYouDontNNeedToSee()
        {
            return "<html><style>.name{"
                    + "float:left;"
                    + " margin: 0px;"
                    + "  border: 0px solid black;"
                    + "}"
                    + " .tooltip{"
                    + "position: absolute;"
                    + " margin: 5px;"
                    + "width: 350px;"
                    + " height: 100px;"
                    + " border: 1px solid black;"
                    + " display: none;"
                    // + "background-color: coral;"
                    + "} "



                    + ".wrapper{"
                    + "\nheight: 100%;"
                    + "\nwidth: 100%;"
                    + "\nleft: 0;"
                    + "\nright: 0;"
                    + "\ntop: 0;"
                    + "\nbottom: 0;"
                    + "\nposition: absolute;"
                    + "\nbackground: linear-gradient(124deg, #ff2400, #e81d1d, #e8b71d, #e3e81d, #1de840, #1ddde8, #2b1de8, #dd00f3, #dd00f3);"
                    + "\nbackground-size: 1800% 1800%;"

                    + "\n-webkit-animation: rainbow 18s ease infinite;"
                    + "\n-z-animation: rainbow 18s ease infinite;"
                    + "\n-o-animation: rainbow 18s ease infinite;"
                    + "\nanimation: rainbow 18s ease infinite;"
                    + "\n}"

                    + "\n@-webkit-keyframes rainbow {"
                    + "\n 0%{ background-position:0% 82%}"
                    + "\n 50%{ background-position:100% 19%}"
                    + "\n 100%{ background-position:0% 82%}"
                    + "\n }"
                    + "\n@-moz-keyframes rainbow {"
                    + "\n 0%{ background-position:0% 82%}"
                    + "\n 50%{ background-position:100% 19%}"
                    + "\n 100%{ background-position:0% 82%}"
                    + "\n}"
                    + "\n@-o-keyframes rainbow {"
                    + "\n 0%{ background-position:0% 82%}"
                    + "\n 50%{ background-position:100% 19%}"
                    + "\n 100%{ background-position:0% 82%}"
                    + "\n}"
                    + "\n@keyframes rainbow {"
                    + "\n 0%{ background-position:0% 82%}"
                    + "\n 50%{ background-position:100% 19%}"
                    + "\n 100%{ background-position:0% 82%}"
                    + "\n}"


                    + "</style><script>"
                    + "function show(elem) {"
                    + "elem.style.display = \"block\";"
                    + "}"
                    + "function hide(elem)"
                    + "{"
                    + "elem.style.display = \"\";"
                    + "}"
                    + "</script>"
                   + "<div class=\"wrapper\"> ";
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
                for (int i = 2; i < manifestLog.Length; i++)
                {
                    if (manifestLog[i].Contains("\\"))
                    {
                        String[] temp = manifestLog[i].Split('\\');
                        Person newPerson = new Person();
                        newPerson.ckey = temp[0].Split(' ')[2].ToLower();
                        newPerson.Name = temp[1].Trim(' ').ToLower();
                        newPerson.Job = temp[2].Trim(' ').ToLower();
                        newPerson.antag = temp[3].Trim(' ').ToLower();
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

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        int i = 0;
        private string displayToolTip(String log, Person tempPerson,String prefix)
        {
            String color = "WHITE";
            if (tempPerson.Job.Contains("Sec".ToLower()) || tempPerson.Job.Contains("Warden".ToLower()))
            {
                color = "RED";
            }
            if (tempPerson.Job.Contains("Engineer".ToLower()) || tempPerson.Job.Contains("Atmos".ToLower()))
            {
                color = "Orange";
            }
            if (tempPerson.Job.Contains("Sci".ToLower()) || tempPerson.Job.Contains("Direct".ToLower()) || tempPerson.Job.Contains("Robot".ToLower()))
            {
                color = "Purple";
            }
            if (tempPerson.Job.Contains("Doc".ToLower()) || tempPerson.Job.Contains("Medi".ToLower()) || tempPerson.Job.Contains("Genet".ToLower()) || tempPerson.Job.Contains("Chemistry".ToLower()))
            {
                color = "cyan";
            }
            i++;
            Boolean isAntag = !(tempPerson.antag.Contains("none"));
            
            log = ((isAntag) ? "<b><u>" : "")+ "" +"<div  class = \"name\" onmouseover=\"show(tooltip" + i + ")\" onmouseout=\"hide(tooltip" + i + ")\" > " 
                  + prefix+" "+tempPerson.Name 
                  + "<div class = \"tooltip\" id= \"tooltip"+i+ "\" style=\"background-color:"+color+";\">"
                  + "<div>IC Name: " + tempPerson.Name + "</div>"
                  + "<div>OOC Name: " + tempPerson.ckey + "</div>"
                  + "<div>JOB: " + tempPerson.Job + "</div>"
                  + "<div>ANTAG: " + tempPerson.antag + "</div>"
                  + "<div>TIME: " + "[" + times[times.Count - 1].ToLongTimeString() + "]" + " </div>"
                  + "</div>"
                  + "</div>" + log+"" + ((isAntag) ? "</b></u>" : "");

            return log;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
