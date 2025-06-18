using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    public enum GradeCategory
    {
        Excellent = 90,
        Good = 80,
        Satisfactory = 70,
        NeedsImprovement = 60,
        Failing = 0
    }

    public struct StudentInfo
    {
        public string Name { get; set; }
        public int Grade { get; set; }
        public GradeCategory Category => GetGradeCategory(Grade);

        public StudentInfo(string name, int grade)
        {
            Name = name;
            Grade = grade;
        }

        // Determines grade category based on numeric score
        public static GradeCategory GetGradeCategory(int grade)
        {
            if (grade >= 90) 
              return GradeCategory.Excellent;
            if (grade >= 80) 
              return GradeCategory.Good;
            if (grade >= 70) 
              return GradeCategory.Satisfactory;
            if (grade >= 60) 
              return GradeCategory.NeedsImprovement;
            return GradeCategory.Failing;
        }
    }

    public partial class StudentManagementForm : Form
    {
        // Store students with name as key
        private Dictionary<string, StudentInfo> students = new Dictionary<string, StudentInfo>();
        private TextBox txtName, txtGrade, txtSearch;
        private ListBox lstStudents;
        private Label lblResults;

        public StudentManagementForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Student Grade Management System";
            Size = new Size(900, 650);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(240, 248, 255);

            // Create UI sections
            var registrationPanel = CreateSection("Student Registration", 20, 20, 400, 140, Color.FromArgb(230, 255, 230));
            
            AddLabelToPanel(registrationPanel, "Student Name:", 15, 35, 100);
            txtName = AddTextBoxToPanel(registrationPanel, 120, 33, 200);
            
            AddLabelToPanel(registrationPanel, "Grade (0-100):", 15, 65, 100);
            txtGrade = AddTextBoxToPanel(registrationPanel, 120, 63, 100);
            
            var btnAdd = AddButtonToPanel(registrationPanel, "Add Student", 240, 62, BtnAdd_Click, Color.FromArgb(144, 238, 144), 120);
            btnAdd.Font = new Font("Arial", 9, FontStyle.Bold);

            var searchPanel = CreateSection("Student Search", 450, 20, 400, 140, Color.FromArgb(240, 248, 255));
            
            AddLabelToPanel(searchPanel, "Student Name:", 15, 35, 100);
            txtSearch = AddTextBoxToPanel(searchPanel, 120, 33, 200);
            
            var btnSearch = AddButtonToPanel(searchPanel, "Search", 240, 62, BtnSearch_Click, Color.FromArgb(173, 216, 230), 120);
            btnSearch.Font = new Font("Arial", 9, FontStyle.Bold);

            var operationsPanel = CreateSection("Student Operations", 20, 180, 830, 100, Color.FromArgb(255, 248, 220));
            
            AddButtonToPanel(operationsPanel, "Display All Students", 15, 35, BtnDisplayAll_Click, Color.FromArgb(255, 228, 196), 150, 35);
            AddButtonToPanel(operationsPanel, "Calculate Average", 180, 35, BtnCalculateAverage_Click, Color.FromArgb(221, 160, 221), 150, 35);
            AddButtonToPanel(operationsPanel, "Find Min/Max Grades", 345, 35, BtnFindMinMax_Click, Color.FromArgb(255, 182, 193), 150, 35);
            AddButtonToPanel(operationsPanel, "Clear All Results", 510, 35, BtnClear_Click, Color.FromArgb(211, 211, 211), 120, 35);
            AddButtonToPanel(operationsPanel, "Remove Student", 645, 35, BtnRemove_Click, Color.FromArgb(255, 160, 160), 120, 35);

            var displayPanel = CreateSection("Students Database", 20, 300, 420, 280, Color.FromArgb(248, 248, 255));
            
            lstStudents = new ListBox
            {
                Location = new Point(15, 35),
                Size = new Size(390, 230),
                Font = new Font("Consolas", 9),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            displayPanel.Controls.Add(lstStudents);

            var resultsPanel = CreateSection("Results & Information", 460, 300, 390, 280, Color.FromArgb(255, 250, 240));
            
            lblResults = new Label
            {
                Location = new Point(15, 35),
                Size = new Size(360, 230),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9),
                Text = "Welcome to Student Grade Management System!\n\nInstructions:\n• Enter student name and grade to register\n• Use search to find specific students\n• Operations provide statistics and management\n• All results will appear here",
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(10)
            };
            resultsPanel.Controls.Add(lblResults);

            // Status bar for quick statistics
            var statusLabel = new Label
            {
                Text = "Ready • Total Students: 0 • Average Grade: N/A",
                Location = new Point(20, 600),
                Size = new Size(830, 25),
                BackColor = Color.FromArgb(220, 220, 220),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Font = new Font("Arial", 9)
            };
            Controls.Add(statusLabel);
            
            UpdateStatusBar(statusLabel);
        }

        private Panel CreateSection(string title, int x, int y, int width, int height, Color backColor)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = backColor
            };

            var titleLabel = new Label
            {
                Text = title,
                Location = new Point(10, 5),
                Size = new Size(width - 20, 25),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 70),
                BackColor = Color.Transparent
            };

            panel.Controls.Add(titleLabel);
            Controls.Add(panel);
            return panel;
        }

        private Label AddLabelToPanel(Panel panel, string text, int x, int y, int width, int height = 20)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = Color.Transparent,
                Font = new Font("Arial", 9)
            };
            panel.Controls.Add(label);
            return label;
        }

        private TextBox AddTextBoxToPanel(Panel panel, int x, int y, int width, int height = 25)
        {
            var textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("Arial", 9)
            };
            panel.Controls.Add(textBox);
            return textBox;
        }

        private Button AddButtonToPanel(Panel panel, string text, int x, int y, EventHandler clickHandler, Color backColor, int width = 100, int height = 25)
        {
            var button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 8),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderColor = Color.Gray;
            button.Click += clickHandler;
            panel.Controls.Add(button);
            return button;
        }

        // Add new student or update existing one
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateInput(txtName.Text, txtGrade.Text, out string name, out int grade);

                // Check for existing student
                if (students.ContainsKey(name))
                {
                    var result = MessageBox.Show($"Student '{name}' exists with grade {students[name].Grade}. Update?",
                                               "Student Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) return;
                }

                var student = new StudentInfo(name, grade);
                students[name] = student;

                ClearInputs();
                UpdateStudentsList();
                UpdateStatusBar();
                ShowResult($"Registration Successful!\n\nStudent: {name}\nGrade: {grade}\nCategory: {student.Category}\n\nTotal Students: {students.Count}");
            }
            catch (Exception ex)
            {
                ShowError($"Registration failed: {ex.Message}");
            }
        }

        // Search for a specific student by name
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchName = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchName))
                    throw new ArgumentException("Please enter a student name to search.");

                if (students.TryGetValue(searchName, out StudentInfo student))
                {
                    ShowResult($"Student Found!\n\nName: {student.Name}\nGrade: {student.Grade}\nCategory: {student.Category}\n\nSearch completed successfully.");
                }
                else
                {
                    ShowResult($"Search Result\n\nStudent '{searchName}' not found in database.\n\nTotal students registered: {students.Count}");
                }
                txtSearch.Clear();
            }
            catch (Exception ex)
            {
                ShowError($"Search failed: {ex.Message}");
            }
        }

        private void BtnDisplayAll_Click(object sender, EventArgs e)
        {
            if (students.Count == 0)
            {
                ShowResult("Database Status\n\nNo students registered yet.\n\nPlease add students using the registration section.");
                return;
            }

            UpdateStudentsList();
            ShowResult($"All Students Retrieved\n\nTotal Students: {students.Count}\nDatabase updated successfully.\n\nCheck the students list for complete details.");
            UpdateStatusBar();
        }

        // Calculate average grade of all students
        private void BtnCalculateAverage_Click(object sender, EventArgs e)
        {
            try
            {
                if (students.Count == 0)
                    throw new InvalidOperationException("No students in database to calculate average.");

                // Calculate average grade using LINQ
                double avg = students.Values.Select(s => s.Grade).Average();
                var avgCategory = StudentInfo.GetGradeCategory((int)Math.Round(avg));

                ShowResult($"Grade Statistics\n\nAverage Grade: {avg:F2}\nAverage Category: {avgCategory}\nTotal Students: {students.Count}\n\nCalculation completed successfully.");
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                ShowError($"Statistics calculation failed: {ex.Message}");
            }
        }

        /// Event handler to find and display students with minimum and maximum grades
        private void BtnFindMinMax_Click(object sender, EventArgs e)
        {
            try
            {
                if (students.Count == 0)
                    throw new InvalidOperationException("No students in database.");

                // Find min and max grades using LINQ
                var grades = students.Values.Select(s => s.Grade);
                int max = grades.Max();
                int min = grades.Min();

                // Find all students with the highest and lowest grades
                var topStudents = string.Join(", ", students.Where(s => s.Value.Grade == max).Select(s => s.Key));
                var bottomStudents = string.Join(", ", students.Where(s => s.Value.Grade == min).Select(s => s.Key));

                ShowResult($"Grade Analysis\n\nHighest Grade: {max}\nTop Performer(s): {topStudents}\n\nLowest Grade: {min}\nNeeds Support: {bottomStudents}\n\nAnalysis completed.");
            }
            catch (Exception ex)
            {
                ShowError($"Grade analysis failed: {ex.Message}");
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // Try to get student name from search field first, then name field
                string studentName = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(studentName))
                {
                    studentName = txtName.Text.Trim();
                    if (string.IsNullOrEmpty(studentName))
                        throw new ArgumentException("Please enter a student name to remove.");
                }

                // Check if student exists and confirm removal
                if (students.ContainsKey(studentName))
                {
                    var result = MessageBox.Show($"Are you sure you want to remove student '{studentName}'?",
                                               "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // Remove student and update displays
                        students.Remove(studentName);
                        UpdateStudentsList();
                        UpdateStatusBar();
                        ShowResult($"Student Removed\n\nStudent '{studentName}' has been removed from database.\n\nRemaining students: {students.Count}");
                        ClearInputs();
                        txtSearch.Clear();
                    }
                }
                else
                {
                    ShowResult($"Removal Failed\n\nStudent '{studentName}' not found in database.\n\nPlease check the name and try again.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Student removal failed: {ex.Message}");
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            lblResults.Text = "Results Cleared\n\nAll results have been cleared.\nReady for new operations.\n\nUse the sections above to manage students.";
            ClearInputs();
            txtSearch.Clear();
        }

        private void ValidateInput(string name, string gradeText, out string validName, out int validGrade)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Student name cannot be empty.");

            if (!int.TryParse(gradeText, out validGrade))
                throw new FormatException("Grade must be a valid integer.");

            if (validGrade < 0 || validGrade > 100)
                throw new ArgumentOutOfRangeException("Grade must be between 0 and 100.");

            validName = name.Trim();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtGrade.Clear();
        }

        private void ShowResult(string message)
        {
            lblResults.Text = message;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblResults.Text = $"Error\n\n{message}";
        }

        private void UpdateStudentsList()
        {
            lstStudents.Items.Clear();

            if (students.Count == 0)
            {
                lstStudents.Items.Add("No students registered");
                return;
            }

            // Add table headers
            lstStudents.Items.Add("Name".PadRight(20) + "Grade".PadRight(8) + "Category");
            lstStudents.Items.Add(new string('═', 50)); // Separator line

            // Add student data sorted alphabetically by name
            foreach (var student in students.OrderBy(s => s.Key).Select(s => s.Value))
            {
                lstStudents.Items.Add($"{student.Name.PadRight(20)}{student.Grade.ToString().PadRight(8)}{student.Category}");
            }
        }

        private void UpdateStatusBar(Label statusLabel = null)
        {
            // Find status label if not provided
            if (statusLabel == null)
                statusLabel = Controls.OfType<Label>().FirstOrDefault(l => l.Text.Contains("Ready"));

            if (statusLabel != null)
            {
                // Calculate average grade or show N/A if no students
                string avgText = students.Count > 0 ? 
                    students.Values.Select(s => s.Grade).Average().ToString("F1") : "N/A";
                statusLabel.Text = $"Ready • Total Students: {students.Count} • Average Grade: {avgText}";
            }
        }
    }

    public class Program
    {
        /// Main application entry point
        /// Initializes Windows Forms application and starts the main form
        [STAThread]
        public static void Main()
        {
            // Enable visual styles for modern Windows appearance
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                // Start the main application form
                Application.Run(new StudentManagementForm());
            }
            catch (Exception ex)
            {
                // Handle any fatal application errors
                MessageBox.Show($"Fatal error: {ex.Message}", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}