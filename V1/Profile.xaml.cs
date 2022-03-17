using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace V1
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public Profile()
        {
            InitializeComponent();
        }

        #region UPDATERS

        #region updateName
        private string name = string.Empty;
        public string updateName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                tb_stdName.Text = name;
            }
        }
        #endregion

        #region update history
        private string history = string.Empty;
        public string updateHistory
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
                tb_history.Text = history;
            }
        }
        #endregion

        #region update symptoms
        private string symptoms = string.Empty;
        public string updateSymptoms
        {
            get
            {
                return symptoms;
            }
            set
            {
                symptoms = value;
                tb_symptoms.Text = symptoms;
            }
        }
        #endregion

        #region update condition
        private string condition  = string.Empty;
        public string updateCondition
        {
            get
            {
                return condition;
            }
            set
            {
                condition = value;
                tb_condition.Text = condition;
            }
        }
        #endregion

        #endregion




    }

}
