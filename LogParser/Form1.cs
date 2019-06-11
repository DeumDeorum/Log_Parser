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
            try
            {
                filters = new List<string>();
                filterOut = new List<string>();

                checkBoxFilters();
                filterBySelectionTool();
                priority = filters.Count;

                filterOutBySelectionTool();

                startFilter();
            }
            catch (Exception) { };


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
        String[] actionLog = new string[0];
        List<String> filterOut = new List<String>();
        private void startFilter()
        {

            List<DateTime> times = new List<DateTime>();
            richTextBox1.Hide();
            String[] gameLog2 = gameLog.Clone() as String[];
            String[] actionLog2 = actionLog.Clone() as String[];
            if (gameLog2 != null || !(gameLog2.Length < 0))
            {

                for (int i = 0; i < gameLog2.Length; i++)
                {
                    Boolean result = false;
                    Boolean filterOutResult = false;
                    Person foundPerson = new Person();
                    String log = gameLog2[i].ToLower();
                    for (int b = 0; b < filters.Count && !result; b++)
                    {
                        result = log.Contains(filters[b].ToLower());

                    }
                    for (int c = 0; c < filterOut.Count && result && !filterOutResult; c++)
                    {
                        filterOutResult = log.Contains(filterOut[c].ToLower());
                    }
                    bool personFound = false;

                    if (log.Contains(")"))
                    {
                        foreach (Person p in listOfPeople)
                        {
                            if (log.Contains("i need a new cmo"))
                                ;
                            personFound = log.Substring(0, log.IndexOf(')')).Contains(p.ckey.ToLower());
                            foundPerson = p;
                            if (personFound)
                                break;
                        }
                        if (log.Contains("i need a new cmo"))
                            ;
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
                        if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(") + 1);
                        if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(") + 1);
                        reverse = logReverse.ToCharArray();
                        Array.Reverse(reverse);
                        log = new string(reverse);
                        string encasePerson = "";
                        if (personFound)
                        {
                            encasePerson = "<Details> <Summary> " + foundPerson.Name + "</Summary> Yeah i contain info </Details>";
                        }

                        //if (log.Contains("(")) log = log.Substring(0, log.IndexOf("(") - 1);
                        
                        if (gameLog2[i].Contains("OOC"))
                        {
                            log = "<font color=\"purple\"\">" + log + "</font>";
                            log = displayToolTip(log, foundPerson, "OOC: ");
                        }
                        if (gameLog2[i].Contains("ATTACK"))
                        {
                            log = "<font color=\"red\"\">" + log + "</font>";
                            log = displayToolTip("OOC: " + log, foundPerson, "Attack");
                        }
                        if (gameLog2[i].Contains("SAY"))
                        {
                            log = "<font color=\"GREEN\"\">" + log + "</font>";
                            log = displayToolTip(log, foundPerson, "Says: ");
                        }

                        //USE FUCKING SPOILER INSTEAD OF TOOL TIP



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

                    gameLog2[i] = "<div><div>" + log + "</div></div>";


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
                    String log = actionLog2[i].ToLower();
                    for (int b = 0; b < filters.Count && !result; b++)
                    {
                        result = log.Contains(filters[b].ToLower());

                    }
                    for (int c = 0; c < filterOut.Count && result && !filterOutResult; c++)
                    {
                        filterOutResult = log.Contains(filterOut[c].ToLower());
                    }
                    bool personFound = false;

                    if (log.Contains(")"))
                    {
                        foreach (Person p in listOfPeople)
                        {
                            personFound = log.Substring(0,log.IndexOf(')')).Contains(p.ckey.ToLower());
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


                        String deeperDive = log.Substring(log.IndexOf("(") + 1);


                        Char[] reverse = log.ToCharArray();
                        Array.Reverse(reverse);
                        String logReverse = new string(reverse);
                        if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(") + 1);
                        if (logReverse.Contains('(')) logReverse = logReverse.Substring(logReverse.IndexOf("(") + 1);
                        reverse = logReverse.ToCharArray();
                        Array.Reverse(reverse);
                        log = new string(reverse);



                        //if (log.Contains("(")) log = log.Substring(0, log.IndexOf("(") - 1);

                        if (actionLog2[i].Contains("OOC"))
                        {
                            log = "<font color=\"purple\"\">" + log + "</font>";
                            log = displayToolTip(log, foundPerson, "OOC: ");
                        }
                        if (actionLog2[i].Contains("ATTACK"))
                        {
                            log = "<font color=\"red\"\">" + log + "</font>";
                            log = displayToolTip(log, foundPerson, "Attack");
                        }
                        if (actionLog2[i].Contains("SAY"))
                        {
                            log = "<font color=\"GREEN\"\">" + log + "</font>";
                            log = displayToolTip(log, foundPerson, "Says: ");
                        }



                        //log = "<div class=\"tooltip\"> <a href=\"test.html\" title=\"test tooltip\">test link</a>  " + log + " a<span class=\"tooltiptext\">" + foundPerson.Job + " " + foundPerson.antag + "</span> </div>";
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
                    if (ShowTimesBox.Checked) log = "<font size=\"2\">" + temp.ToShortTimeString() + "</font> " + log;
                    if (!result || filterOutResult || foundPerson.Name.Contains("N00000000000N"))
                        log = "";
                    actionLog2[i] = "<div><div>"+log+"</div></div>";
                    


                }
                //   foreach (String t in gameLog)
                //  {ff
                // richTextBox1.Text = String.Join("\n\n", gameLog2.Where((s => !String.IsNullOrEmpty(s))));


            }
            String[] master = new string[gameLog2.Length + actionLog2.Length];
            int f = 0;
            for (int a = 0; a < gameLog.Length; a++)
            {
                Boolean added = false;
                while (f < actionLog.Length)
                {
                    if (times[a] < times[f + gameLog.Length])
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
            if (master.Length > 0)
            {
                //IHTMLDocument2 document = new HtmlDocument();
                //  document.createStyleSheet(           
                webBrowser1.DocumentText = "<html><style>.name{"
                    +   "float:left;"
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
                    + "}"
                    + "</style><script>"
                    + "function show(elem) {"
                    + "elem.style.display = \"block\";"
                    + "}"
                    + "function hide(elem)"
                    + "{"
                    + "elem.style.display = \"\";"
                    + "}"
                    + "</script>"
                    + String.Join("", master.Where((s => handleWhere(s)))) + "<html>";
            }

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
            
            log = ((isAntag) ? "<b><u>" : "")+ "<div>" +"<div  class = \"name\" onmouseover=\"show(tooltip" + i + ")\" onmouseout=\"hide(tooltip" + i + ")\" > " 
                  + prefix+" "+tempPerson.Name 
                  + "  <div class = \"tooltip\" id= \"tooltip"+i+ "\" style=\"background-color:"+color+";\">"
                  + "<div>IC Name: " + tempPerson.Name + "</div>"
                  + "<div>OOC Name: " + tempPerson.ckey + "</div>"
                  + "<div>JOB: " + tempPerson.Job + "</div>"
                  + "<div>ANTAG: " + tempPerson.antag + "</div>"
                  + "   </div>"
                  + "</div>" + log+"</div>" + ((isAntag) ? "</b></u>" : "");





            return log;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
