using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YogaAppV3._0
{
    class YogaAppUtility
    {
        //Used to assign an Image to the picture boxes.
        YogaAppPoseImage[] poseImagesArray = 
        {
            new YogaAppPoseImage { name = "Bow Pose", image = Properties.Resources.BowPose},
            new YogaAppPoseImage { name = "Camel Pose", image = Properties.Resources.CamelPose},
            new YogaAppPoseImage { name = "Cat & Cow Pose", image = Properties.Resources.CatAndCowPose},
            new YogaAppPoseImage { name = "Child's Pose", image = Properties.Resources.ChildsPose},
            new YogaAppPoseImage { name = "Cow Face Pose", image = Properties.Resources.CowFacePose},
            new YogaAppPoseImage { name = "Downward Dog", image = Properties.Resources.DownwardDogPose},
            new YogaAppPoseImage { name = "Dragon Pose", image = Properties.Resources.DragonPose},
            new YogaAppPoseImage { name = "Frog Pose", image = Properties.Resources.FrogPose},
            new YogaAppPoseImage { name = "Happy Baby", image = Properties.Resources.HappyBabyPose},
            new YogaAppPoseImage { name = "King Pigeon", image = Properties.Resources.KingPigeonPose},
            new YogaAppPoseImage { name = "Lizard Pose", image = Properties.Resources.LizardPose},
            new YogaAppPoseImage { name = "Lying Pigeon", image = Properties.Resources.LyingPigeonPose},
            new YogaAppPoseImage { name = "Mountain Pose", image = Properties.Resources.MountainPose},
            new YogaAppPoseImage { name = "Pigeon", image = Properties.Resources.PigeonPose},
            new YogaAppPoseImage { name = "Plow Pose", image = Properties.Resources.PlowPose},
            new YogaAppPoseImage { name = "Saddle Pose", image = Properties.Resources.SaddlePose},
            new YogaAppPoseImage { name = "Shoulder Stand", image = Properties.Resources.ShoulderStandPose},
            new YogaAppPoseImage { name = "Thread The Needle", image = Properties.Resources.ThreadTheNeedlePose},
            new YogaAppPoseImage { name = "Upward-Facing Dog", image = Properties.Resources.UpwardFacingDogPose},
            new YogaAppPoseImage { name = "Warrior 2", image = Properties.Resources.Warrior2Pose},
            new YogaAppPoseImage { name = "Warrior 3", image = Properties.Resources.Warrior3Pose}
        };
        /*
        # Method to assign pictures to the PictureBox passed in.
        # @picBox is passed in to be assigned a picture to display.
        # @picture is the string value of the name of the picture.
        */
        public void assignImageToPicBox(PictureBox picBox, string picture)
        {
            try
            {
                for (int i = 0; i < poseImagesArray.Length; i++)
                {
                    if (poseImagesArray[i].name.Equals(picture))
                    {
                        picBox.Image = poseImagesArray[i].image;
                        break;
                    }
                }
                //picBox.Image = Image.FromFile(Path.Combine(
                //    Environment.CurrentDirectory, @"Resources\", picture));
            }
            catch (IOException iOEx)
            {
                MessageBox.Show("The picture files could not be found!\nThe location should be in:\n" + iOEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry an error has occured:\n" + ex.Message);
            }
        }

        
        public void switchPanel(Panel targetPanel)
        {
           
        }
    }

    // This class is only here to allow for the 
    // poseImagesArray array.
    class YogaAppPoseImage
    {
        public string name { get; set; }
        public Image image { get; set; }
    }
}
