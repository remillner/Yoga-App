/* 
# Written by Rebecca Millner and David Palermo
#
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace YogaAppV3._0
{
    public partial class Form1 : Form
    {

        int index = 0, overallIndex = 0;  //Set in the btnNewSession_Click and used to stop the iterate through poses / stop timer 
        int stretchTime, numPoses;  //Set in TryParse methods in btnNewSession_Click
        int calculatedTime;  //Set in the btnNewSession_Click method and is used in setting the timer interval
        tblUser currentUser;
        tblUserLog currentUserLog;
        List<tblPos> poses = new List<tblPos>();  // allows for import of the tblPoses Table Data
        string[] meditations = { "your breathing", "your heartbeat", "the sky", "a cow atop a hill", "the wind", "your body in contact with the floor" };
        Random rand = new Random();
        YogaAppUtility UtilityAccess = new YogaAppUtility();

        public Form1()
        {
            InitializeComponent();
        }
        
        private void userSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlLogIn.Visible = false;
            pnlPoseBreakdowns.Visible = false;
            pnlRegistration.Visible = false;
            pnlUserHome.Visible = false;
            pnlUserSettings.Visible = true;
        }

        private void poseBreakdownsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlLogIn.Visible = false;
            pnlPoseBreakdowns.Visible = true;
            pnlRegistration.Visible = false;
            pnlUserHome.Visible = false;
            pnlUserSettings.Visible = false;
        }

        private void userHomeScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlLogIn.Visible = false;
            pnlPoseBreakdowns.Visible = false;
            pnlRegistration.Visible = false;
            pnlUserHome.Visible = true;
            pnlUserSettings.Visible = false;
        }

        private void registrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlLogIn.Visible = false;
            pnlPoseBreakdowns.Visible = false;
            pnlRegistration.Visible = true;
            pnlUserHome.Visible = false;
            pnlUserSettings.Visible = false;
        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pnlLogIn.Visible = true;
            pnlPoseBreakdowns.Visible = false;
            pnlRegistration.Visible = false;
            pnlUserHome.Visible = false;
            pnlUserSettings.Visible = false;
            userSettingsToolStripMenuItem.Enabled = false;
            poseBreakdownsToolStripMenuItem.Enabled = false;
            userHomeScreenToolStripMenuItem.Enabled = false;
            registrationToolStripMenuItem.Enabled = true;
        }

        private void btnUserSettings_Click(object sender, EventArgs e)
        {
            using (YogaAppDatabase3Entities context = new YogaAppDatabase3Entities())
            {
                // Assign all the values from the panel into an instance of 
                // tblUserSettings Class in order to add the new information
                // into the TblUserSettings Table
                tblUserSetting userSettings = new tblUserSetting
                {
                    UserID = currentUser.UserID,
                    Pregnant = chkBoxPregnantY.Checked,
                    Injured = chkBoxInjury.Checked,
                    DifficultyLvl = grpBoxLvls.Controls.OfType<RadioButton>()
                                    .FirstOrDefault(n => n.Checked).Text,
                    Injury = comBoxInjuries.SelectedText,
                    Meditation = chkBoxMeditationY.Checked
                };
                
                var previousSettings = from tblUserSetting in context.tblUserSettings
                                    where tblUserSetting.UserID == currentUser.UserID
                                    select tblUserSetting;   

                //Check if a previous User Setting was present during the query
                if (previousSettings.Count() == 1)
                {
                    //Remove if already existed
                    context.tblUserSettings.Remove((from tblUserSetting in context.tblUserSettings
                                                   where tblUserSetting.UserID == currentUser.UserID
                                                   select tblUserSetting).First<tblUserSetting>() );  
                }
                // Add new User Setting to the tblUserSetting Table            
                context.tblUserSettings.Add(userSettings);
                context.SaveChanges();
            }
            pnlUserSettings.Visible = false;
            pnlUserHome.Visible = true;
            tblUserLogsTableAdapter.FillBy(yogaAppDatabase3DataSet.tblUserLogs, currentUser.UserID);
        }

        private void comBoxBeginnerPoses_SelectedIndexChanged(object sender, EventArgs e)
        {
            comBoxExpertPoses.Text = "Expert Poses";
            comBoxIntermediatePoses.Text = "Intermediate Poses";

            using (YogaAppDatabase3Entities context = new YogaAppDatabase3Entities())
            {
                // Query Pose Selected on pnlPoseBreakdown Panel through LINQ
                // then post it where needed 
                var selectedPose = (from tblPos in context.tblPoses
                                    where tblPos.PoseName.Equals(comBoxBeginnerPoses.Text)
                                    select tblPos).First<tblPos>();
                UtilityAccess.assignImageToPicBox(picBoxPoseBreakdowns, selectedPose.PoseName);
                lblPoseBreakdown.Text = selectedPose.BreakDown;
            }
        }
        
        private void comBoxIntermediatePoses_SelectedIndexChanged(object sender, EventArgs e)
        {
            comBoxExpertPoses.Text = "Expert Poses";
            comBoxBeginnerPoses.Text = "Beginner Poses";

            using (YogaAppDatabase3Entities context = new YogaAppDatabase3Entities())
            {
                // Query Pose Selected on pnlPoseBreakdown Panel through LINQ
                // then post it where needed 
                var selectedPose = (from tblPos in context.tblPoses
                                    where tblPos.PoseName.Equals(comBoxIntermediatePoses.Text)
                                    select tblPos).First<tblPos>();
                UtilityAccess.assignImageToPicBox(picBoxPoseBreakdowns, selectedPose.PoseName);
                lblPoseBreakdown.Text = selectedPose.BreakDown;
            }
        }

        private void comBoxExpertPoses_SelectedIndexChanged(object sender, EventArgs e)
        {
            comBoxIntermediatePoses.Text = "Intermediate Poses";
            comBoxBeginnerPoses.Text = "Beginner Poses";

            using (YogaAppDatabase3Entities context = new YogaAppDatabase3Entities())
            {
                // Query Pose Selected on pnlPoseBreakdown Panel through LINQ
                // then post it where needed 
                var selectedPose = (from tblPos in context.tblPoses
                                    where tblPos.PoseName.Equals(comBoxExpertPoses.Text)
                                    select tblPos).First<tblPos>();
                UtilityAccess.assignImageToPicBox(picBoxPoseBreakdowns, selectedPose.PoseName);
                lblPoseBreakdown.Text = selectedPose.BreakDown;
            }
        }

        private void btnNewSession_Click(object sender, EventArgs e)
        {
            overallIndex = 0;
            tblUserSetting currentUserSettings;
            string targetArea;

            try
            {
                int.TryParse(txtBoxTime.Text, out stretchTime);
                int.TryParse(txtBoxNumPoses.Text, out numPoses);
                calculatedTime = stretchTime / numPoses;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter a positive number.\nError: " + ex.Message);
            }
            finally
            {
                txtBoxTime.Text = "";
                txtBoxNumPoses.Text = "";
            }
            if (calculatedTime > 0)  //Sets a minimum requirement to proceed
            {
                using (YogaAppDatabase3Entities context = new YogaAppDatabase3Entities())
                {

                    currentUserSettings = (from tblUserSetting in context.tblUserSettings
                                           where tblUserSetting.UserID.Equals(currentUser.UserID)
                                           select tblUserSetting).First<tblUserSetting>();

                    targetArea = groupBoxAreas.Controls.OfType<RadioButton>()
                                           .FirstOrDefault(n => n.Checked).Text;

                    //Collecting the mass list for full body selections
                    if (targetArea.Equals("Full"))
                    {
                        switch (currentUserSettings.DifficultyLvl)
                        {
                            case "Beginner":
                                poses = (from tblPos in context.tblPoses
                                         where tblPos.Level.Equals(currentUserSettings.DifficultyLvl)
                                         select tblPos).ToList<tblPos>();
                                break;
                            case "Intermediate":
                                // pass poseLvlBeginner along with poseLvl 
                                poses = (from tblPos in context.tblPoses
                                         where tblPos.Level != "Advanced"
                                         select tblPos).ToList<tblPos>();
                                break;
                            case "Advanced":
                                // Just select all in the query
                                poses = (from tblPos in context.tblPoses
                                         select tblPos).ToList<tblPos>();
                                break;
                            default:
                                //Default will assume you are not a beginner
                                MessageBox.Show("There was an error regarding Difficulty Settings\nPlease Check your User Settings");
                                break;
                        }
                    }
                    else
                    {
                        switch (currentUserSettings.DifficultyLvl)
                        {
                            case "Beginner":
                                poses = (from tblPos in context.tblPoses
                                         where tblPos.BodyRegion.Equals(targetArea) &&
                                          tblPos.Level.Equals(currentUserSettings.DifficultyLvl)
                                         select tblPos).ToList<tblPos>();
                                break;
                            case "Intermediate":
                                // pass poseLvlBeginner along with poseLvl 
                                poses = (from tblPos in context.tblPoses
                                         where tblPos.BodyRegion.Equals(targetArea) &&
                                         tblPos.Level != "Advanced"
                                         select tblPos).ToList<tblPos>();
                                break;
                            case "Advanced":
                                // Just select all in the query
                                poses = (from tblPos in context.tblPoses
                                         where tblPos.BodyRegion.Equals(targetArea)
                                         select tblPos).ToList<tblPos>();
                                break;
                            default:
                                //Default will assume you are not a beginner
                                MessageBox.Show("There was an error regarding Difficulty Settings\nPlease Check your User Settings");
                                break;
                        }
                    }
                }

                if (currentUserSettings.Meditation.Value)
                    lblMeditationTxt.Text = String.Format("Consider {0} during this session.",
                        meditations[rand.Next(0, meditations.Length)]);

                pnlUserHome.Visible = false;
                pnlStretch.Visible = true;
                timerStretchPnl.Interval = 60000 * calculatedTime;  
                lblStretchInfo.Text = poses[index].BreakDown;
                UtilityAccess.assignImageToPicBox(picBoxStretch, poses[index].PoseName);
                index++;
                overallIndex = 0;
                timerStretchPnl.Start();
                currentUserLog = new tblUserLog
                {
                    UserID = currentUser.UserID,
                    LogAreaStretched = targetArea,
                    LogLvl = currentUserSettings.DifficultyLvl,
                    LogNumStretch = numPoses.ToString(),
                    LogTime = TimeSpan.FromMinutes(stretchTime),
                    LogTimePer = TimeSpan.FromMinutes(calculatedTime),
                    TimeStamp = DateTime.Now.ToString()
                };
            }
            else
                MessageBox.Show("Minimum time per pose is 1 min.\n" +
                    "Please ensure the number of poses entered\n" +
                    "along with the number of minutes meets the\nrequirement.");
        }

        /*
        # Method for each timer tick that cycles through the yoga
        # poses and the pose information. Also plays a sound in 
        # order to let the user know to switch poses.
        */
        private void timerStretchPnl_Tick(object sender, EventArgs e)
        {
            overallIndex++;
            if (index == poses.Count)
                index = 0;
            System.Media.SystemSounds.Exclamation.Play(); 
            lblStretchInfo.Text = poses[index].BreakDown;
            UtilityAccess.assignImageToPicBox(picBoxStretch, poses[index].PoseName);
            index++;
            if (overallIndex == numPoses)
            {
                timerStretchPnl.Stop();
                lblStretchInfo.Text = "";
                lblMeditationTxt.Text = "";
                picBoxStretch.Image = null;
                pnlStretch.Visible = false;
                pnlUserHome.Visible = true;

                //Update and display the tblUserLogs
                using (YogaAppDatabase3Entities context = new YogaAppDatabase3Entities())
                {
                    context.tblUserLogs.Add(currentUserLog);
                    context.SaveChanges();
                }
                tblUserLogsTableAdapter.FillBy(yogaAppDatabase3DataSet.tblUserLogs, currentUser.UserID);
            }
        }

        /*
        * Method that performs all the necessary checks to ensure User has an account
        * and assigns UserID and Password to an instance of the tblUser Class.
        * After the user is identified the method advances the user to the pnlUserHome
        * panel and loads into the table a query from tblUserLogs by UserID.
        */
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            using (YogaAppDatabase3Entities context = new YogaAppDatabase3Entities())
            {
                //query tblUser Table
                var userLogIn = from tblUser in context.tblUsers
                                where tblUser.UserID == txtBoxUserNameLogIn.Text.Trim()
                                && tblUser.Password == txtBoxPasswordLogIn.Text.Trim()
                                select tblUser;

                //Check if the UserID was present during the query
                if (userLogIn.Count() == 1)
                {
                    //Capture the UserID information to use throughout the App
                    currentUser = new tblUser { UserID = txtBoxUserNameLogIn.Text.Trim(), Password = txtBoxPasswordLogIn.Text.Trim() };

                    pnlUserHome.Visible = true;
                    pnlLogIn.Visible = false;
                    txtBoxPasswordLogIn.Text = "";
                    txtBoxUserNameLogIn.Text = "";
                    userSettingsToolStripMenuItem.Enabled = true;
                    poseBreakdownsToolStripMenuItem.Enabled = true;
                    userHomeScreenToolStripMenuItem.Enabled = true;
                    registrationToolStripMenuItem.Enabled = false;
                    tblUserLogsTableAdapter.FillBy(yogaAppDatabase3DataSet.tblUserLogs, currentUser.UserID);
                }
                else
                    txtBoxPasswordLogIn.Text = "";
            }
        }

        private void tblUserLogsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tblUserLogsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.yogaAppDatabase3DataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'yogaAppDatabase3DataSet.tblUserLogs' table. You can move, or remove it, as needed.
            // this.tblUserLogsTableAdapter.Fill(this.yogaAppDatabase3DataSet.tblUserLogs);

        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            pnlRegistration.Visible = true;
            pnlLogIn.Visible = false;
            txtBoxUserName.Text = txtBoxUserNameLogIn.Text.Trim();
            txtBoxUserNameLogIn.Text = "";
            registrationToolStripMenuItem.Enabled = false;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtBoxUserPassword.Text.Trim().Equals(txtBoxConfirmPassword.Text.Trim()))
            {
                using (YogaAppDatabase3Entities context = new YogaAppDatabase3Entities())
                {
                    // Assign all the values from the txtBoxes into an instance of 
                    // tblUser Class in order to add the new information
                    // into the TblUser Table
                    tblUser newUser = new tblUser
                    {
                        UserID = txtBoxUserName.Text.Trim(),
                        Password = txtBoxUserPassword.Text.Trim()
                    };
                    context.tblUsers.Add(newUser);
                    context.SaveChanges();
                }

                currentUser = new tblUser { UserID = txtBoxUserName.Text.Trim(), Password = txtBoxConfirmPassword.Text.Trim() };
                pnlRegistration.Visible = false;
                pnlUserSettings.Visible = true;
                menuStrip1.Visible = true;
                txtBoxUserPassword.Text = "";
                txtBoxConfirmPassword.Text = "";
                txtBoxUserName.Text = "";
                userSettingsToolStripMenuItem.Enabled = true;
                poseBreakdownsToolStripMenuItem.Enabled = true;
                userHomeScreenToolStripMenuItem.Enabled = true;
                registrationToolStripMenuItem.Enabled = false;
            }
            else
            {
                txtBoxUserPassword.Text = "";
                txtBoxConfirmPassword.Text = "";
            }
        }
    }
}
