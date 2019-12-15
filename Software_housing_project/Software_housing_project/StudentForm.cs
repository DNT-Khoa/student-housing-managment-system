﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Software_housing_project
{
    public partial class StudentForm : Form
    {
        private int complaintTracker = House.complaints.Count;

        public StudentForm()
        {
            House.tenants.Add(new Student("Marta", "Alston", 15, "Fontys", "ICT"));
            House.tenants.Add(new Student("Jim", "Bob", 19, "Fontys", "ICT"));
            House.tenants.Add(new Student("Elvis", "Presley", 20, "Fontys", "ICT"));
              
            
            InitializeComponent();
            mcEvents.MinDate = DateTime.Now;
            mcChore.MinDate = DateTime.Now;
            UpdateCheckBoxStudentsName();
            UpdateChoresDescriptions();
        }

        private void btnAddChore_Click(object sender, EventArgs e)
        {
            string selectedDate = mcChore.SelectionRange.Start.ToShortDateString();
            int result = DateTime.Compare(mcChore.SelectionRange.Start, mcChore.TodayDate);
            if (cbxName.SelectedIndex == -1)
            {
                MessageBox.Show("Select a correct student name!");
            }
            else if(cbxChore.SelectedIndex == -1)
            {
                MessageBox.Show("Select a correct chore!");
            }
            else if(String.Compare(selectedDate, string.Empty) == 0)
            {
                MessageBox.Show("Select a correct date!");
            } else if (result < 0)
            {
                MessageBox.Show("You cannot choose a date from the past! It doesn't make sense.");
            }
            else
            {
                if (choreIsValid(cbxChore.SelectedItem.ToString(), cbxName.SelectedItem.ToString(), selectedDate))
                {
                    Chore chore = new Chore(cbxChore.SelectedItem.ToString(), cbxName.SelectedItem.ToString(), selectedDate);

                    House.chores.Add(chore);

                    clbChores.Items.Add(chore.GetInfo());

                    House.updateChores();
                } else
                {
                    MessageBox.Show("The same person should not do the same chore on the same day more than one time ! Do you think so ?");
                }
                
            }    
        }

        private bool choreIsValid(string choreName, string studentName, string choreDate)
        {
            foreach(Chore chore in House.chores)
            {
                if (chore.getChoreDescription() == choreName && chore.getChoreStudentName() == studentName && chore.getChoreDate() == choreDate)
                {
                    return false;
                } 
            }
            return true;
        }
        //Rules page
        public void updateRules()
        {
            List<string> internalRules = GetRules();
            int lineIndex = 0;
            if (internalRules != House.rules)
            {
                lbxRules.Items.Clear();
                foreach (string rule in House.rules)
                {
                    lbxRules.Items.Add(lineIndex.ToString() + ". " + rule);
                    lineIndex++;
                }
            }
        }
        //Rules page
        private List<string> GetRules()
        {
            List<string> internalRules = new List<string>();
            foreach (string rule in lbxRules.Items)
            {
                internalRules.Add(rule);
            }
            return internalRules;
        }
        public void UpdateCheckBoxStudentsName()
        {
            cbxName.Items.Clear();
            cbxEventHost.Items.Clear();
            foreach (var student in House.tenants)
            {
                cbxName.Items.Add($"{student.FirstName} -- Id:{student.IdNumber}");
                cbxEventHost.Items.Add($"{student.FirstName} -- Id:{student.IdNumber}");
            }
        }

        private void UpdateChoresDescriptions()
        {
            cbxChore.Items.Add("Wash the dishes.");
            cbxChore.Items.Add("Take out the garbage.");
            cbxChore.Items.Add("Go grocery shopping.");
            cbxChore.Items.Add("Clean toilet.");
        }

        private void btnFileComplaint_Click(object sender, EventArgs e)
        {
            string selectedDate = mcComplaint.SelectionRange.Start.ToShortDateString(); 

            if (String.Compare(rtbDescription.Text, string.Empty) == 0)
            {
                MessageBox.Show("Complaint can't be blank!");
            }
            else if (String.Compare(selectedDate, string.Empty) == 0)
            {
                MessageBox.Show("Select a correct date!");
            }
            else
            {
                Complaint complaint = new Complaint(selectedDate, rtbDescription.Text);
                House.complaints.Add(complaint);
                rtbComplaints.Text = complaint.GetInfo();
                House.updateComplaints();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if(House.complaints.Count != 0)
            {
                
                if (complaintTracker >= 0 && complaintTracker < House.complaints.Count)
                {
                    rtbComplaints.Text = House.complaints[complaintTracker].GetInfo();
                    complaintTracker--;

                }
                else
                {
                    complaintTracker = House.complaints.Count - 1;
                }
            }
            else
            {
                MessageBox.Show("There are no complaints!");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(House.complaints.Count != 0)
            {
                if(complaintTracker < House.complaints.Count && complaintTracker >= 0)
                {
                    rtbComplaints.Text = House.complaints[complaintTracker].GetInfo();
                    complaintTracker++;
                }
                else
                {
                    complaintTracker = 0;
                    
                }
            }
            else
            {
                MessageBox.Show("There are no complaints!");
            }
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {                      
            if(cbxEventHost.SelectedIndex != -1 && cbxEventHost.Text != "")
            {
                string name = cbxEventHost.SelectedItem.ToString();
                if(tbxEventTitle.Text != "" && tbxEventTitle.Text != "Event Title")
                {
                    string title = tbxEventTitle.Text;
                    if(mcEvents.SelectionRange.Start.ToShortDateString() != "")
                    {
                        string date = mcEvents.SelectionRange.Start.ToShortDateString();
                        if (rtbEventDescription.Text != "" && rtbEventDescription.Text != "Description")
                        {
                            string description = rtbEventDescription.Text;
                            House.events.Add(new Event(name, title, date, description));
                            clbEvents.Items.Add($"{date} {title} : {name}");
                        }
                        else
                        {
                            MessageBox.Show("Please enter a proper description");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a date");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a title");
                }
            }
            else
            {
                MessageBox.Show("Please enter a student");
            }
        }

        private void btnShowInfoEvent_Click(object sender, EventArgs e)
        {
            int index = clbEvents.SelectedIndex;
            string info = House.events[index].GetInfo();
            MessageBox.Show(info);
        }


        public void updateComplaints()
        {
            rtbComplaints.Clear();
            rtbComplaints.Text = House.complaints[complaintTracker].GetInfo();
        }
        
        

        private void clbChores_DoubleClick(object sender, EventArgs e)
        {
            House.chores.RemoveAt(this.clbChores.SelectedIndex);
            this.clbChores.Items.Remove(this.clbChores.SelectedItem);
            House.updateChores();
        }

        private void tbxEventTitle_Click(object sender, EventArgs e)
        {
            tbxEventTitle.Clear();
        }
        private void rtbEventDescription_Click(object sender, EventArgs e)
        {
            rtbEventDescription.Clear();
        }
    }
}
