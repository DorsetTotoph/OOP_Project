﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
namespace OOP_Project
{
    public class FacilityMember : User, IPersonalInformations
    {
        public string name { get; set; }
        public string lastname { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string birthDate { get; set; }
        public int numLevel { get; set; }
        public int level2 { get; set; }
        public int class2 { get; set; }
        public List<string>[] disponibility = new List<string>[7];
        public int NumClass { get; set; }
        List<Subject> subjectTaught = new List<Subject>();
        public List<Subject> SubjectTaught
        {
            get
            {
                return this.SubjectTaught = subjectTaught;
            }
            set
            {
                value = subjectTaught;
            }
        }
        List<LevelAndClasses> levelandclasses = new List<LevelAndClasses>();
        public List<LevelAndClasses> Levelandclasses
        {
            get
            {
                return this.Levelandclasses = levelandclasses;
            }
            set
            {
                value = levelandclasses;
            }
        }
        public List<Classes> classes = new List<Classes>();
        public List<Classes> ClassesStudent
        {
            get { return this.ClassesStudent = classes; }

            set { value = classes; }

        }
        public List<Student> classe = new List<Student>();
        public List<Student> Class
        {
            get { return this.Class = classe; }

            set { value = classe; }

        }



        // return true if the login and password are the good ones and also if the category(facilityMember) is true
        public override bool Login() //complete 
        {
            bool access = false;
            StreamReader reader = new StreamReader(pathAccessibilityLevel); // declaration of the reader and the link of the file
            string temp = " ";
            while (temp != null)
            {
                temp = reader.ReadLine();
                if (temp == null) break;
                string[] columns = temp.Split(';');
                if (columns[0] == login && columns[1] == password && columns[2] == "facilityMember")// comparison between the datas of the file and the data given by the user 
                {
                    access = true;
                }

            }
            reader.Close();// closing of the streamreader
            return access;
        }

        // extract student data from the database (student file) 
        public override void ExtractData() // à terminer lorsque le fichier student aura été créé
        {
            StreamReader reader = new StreamReader(pathFacilityMember);
            string temp = " ";
            while (temp != null)
            {
                temp = reader.ReadLine();
                if (temp == null) break;
                string[] columns = temp.Split(';');
                if (login == columns[2])
                {
                    name = columns[0];
                    lastname = columns[1];
                    mail = columns[2];
                    phone = columns[3];
                }
            }
            reader.Close();// closing of the streamreader
        }

        public FacilityMember() : base()
        {
            Console.WriteLine("Login ?");
            login = Console.ReadLine();
            Console.WriteLine("Password ?");
            password = Console.ReadLine();
            while (Login() == false)
            {
                if (Login() == true) break;
                Console.WriteLine("Login ?");
                login = Console.ReadLine();
                Console.WriteLine("Password ?");
                password = Console.ReadLine();
            }

            for (int i = 0; i < 7; i++) disponibility[i] = new List<string>();
            ReadDispo();
            StreamReader readTeacher = new StreamReader(pathFacilityMember);
            string temp2 = "";
            while (temp2 != null)
            {
                temp2 = readTeacher.ReadLine();
                if (temp2 == null) break;
                string[] rTeacher = temp2.Split(';');
                if (rTeacher[2] == login)
                {
                    this.name = rTeacher[0];
                    this.lastname = rTeacher[1];
                    this.login = rTeacher[2];
                    this.phone = rTeacher[3];
                    string[] rTeacher2 = rTeacher[4].Split(',');
                    // subjects taught
                    for (int i = 0; i < rTeacher2.Length; i++)
                    {
                        SubjectTaught.Add((Subject)Convert.ToInt32(rTeacher2[i]));
                    }

                    // level and classes
                    for (int j = 5; j < rTeacher.Length; j++)
                    {
                        LevelAndClasses level = new LevelAndClasses();
                        string[] splitTab = rTeacher[j].Split(',');
                        level.Level = (Convert.ToInt32(splitTab[0]));
                        for (int k = 1; k < splitTab.Length; k++)
                        {
                            level.Classes.Add(Convert.ToInt32(splitTab[k]));
                        }

                        Levelandclasses.Add(level);
                    }
                    break;
                }



            }
            readTeacher.Close();

            Classes classe = new Classes();
            for (int i = 0; i < Levelandclasses.Count; i++)
            {
                for (int j = 0; j < Levelandclasses[i].Classes.Count; j++)
                {

                    classe = new Classes();
                    classe.Level = Levelandclasses[i].Level;
                    classe.ClassGroup = Levelandclasses[i].Classes[j];
                    StreamReader readStudent = new StreamReader(pathStudent);
                    string temp = "";
                    while (temp != null)
                    {
                        temp = readStudent.ReadLine();
                        if (temp == null) break;
                        string[] rstudent = temp.Split(';');
                        if (Convert.ToInt32(rstudent[8]) == Levelandclasses[i].Level && Convert.ToInt32(rstudent[9]) == Levelandclasses[i].Classes[j])
                        {
                            bool ctor = false;
                            Student student = new Student(ctor);
                            student.name = rstudent[0];
                            student.lastname = rstudent[1];
                            student.mail = rstudent[2];
                            student.StudentID = rstudent[3];
                            student.birthDate = rstudent[4];
                            student.Absences = Convert.ToInt32(rstudent[5]);
                            student.phone = rstudent[6];
                            if (rstudent[7] == "0") student.Tutor = false;
                            else student.Tutor = true;
                            student.GradeLevel = Convert.ToInt32(rstudent[8]);
                            student.Workgroup = Convert.ToInt32(rstudent[9]);
                            student.UnpaidFees = Convert.ToDouble(rstudent[10]);
                            student.NbSubject = Convert.ToInt32(rstudent[11]);
                            for (int l = 12; l < 12 + student.NbSubject; l++)
                            {
                                student.GradesTemp = student.GradesTemp + ";" + rstudent[l];
                            }
                            for (int l = 12; l < 12 + student.NbSubject; l++)
                            {
                                string[] splitTab = rstudent[l].Split(',');
                                if (rstudent[l].Length > 1)
                                {
                                    student.Courses.Add((Subject)Convert.ToInt32(splitTab[0]));
                                    int k = 0;
                                    while (k < splitTab.Length)
                                    {
                                        Exam exam = new Exam();
                                        exam.Sub = Convert.ToInt32(splitTab[k]);
                                        exam.Mark = Convert.ToDouble(splitTab[k + 1]);
                                        exam.Coef = Convert.ToDouble(splitTab[k + 2]);
                                        exam.Date = splitTab[k + 3];
                                        student.Grades.Add(exam);
                                        k = k + 4;
                                    }
                                }
                                else
                                {
                                    Subject adding = new Subject();
                                    adding = (Subject)Convert.ToInt32(Convert.ToInt32(rstudent[l]));
                                    student.Courses.Add(adding);
                                }
                            }
                            string tut = null;
                            if (student.Tutor == false) tut = "0";
                            else tut = "1";
                            student.Data = ($"{student.name};{student.lastname};{student.mail};{student.StudentID};{student.birthDate};{student.Absences};{phone};{tut};{student.GradeLevel};{student.Workgroup};{student.UnpaidFees};{student.NbSubject}{student.GradesTemp}");
                            classe.ClassStudent.Add(student);

                        }
                    }
                    readStudent.Close();
                    ClassesStudent.Add(classe);
                }
            }
            DesignationOfLevelAndClass();
            Console.Clear();

        }

        public void ExeFunctions()
        {
            string carryOn = "N";
            int compt = 0;
            while (carryOn == "N")
            {
                DisplayPersonalInfos();
                if (compt > 0) DesignationOfLevelAndClass();
                compt++;
                Console.WriteLine("1. Add new notes to a class\n" +
                    "2. Display tutor list\n" +
                    "3. Display Attendance of a student\n" +
                    "4. Display student results\n" +
                    "5. Display Exams of the year\n" +
                    "6. Add exam assignments\n" +
                    "7. Give absences to student\n" +
                    "8. Take off absences of a student\n" +
                    "9. Add Disponibility\n" +
                    "10. Delete disponibilities\n" +
                    "11. Edit Disponibilities\n" +
                    "12. Display Disponibilities\n" +
                    "13. Display tutor information\n" +
                    "14. Mean of an exam\n" +
                    "15. Display the name of the students in the class\n" +
                    "16. Disconnect\n");

                int switchCase = Convert.ToInt32(Console.ReadLine());
                switch (switchCase)
                {
                    case 1:
                        AddResultsExam();
                        break;
                    case 2:
                        DisplayTutorList();
                        break;
                    case 3:
                        Console.WriteLine("First name of the student");
                        string fN = Console.ReadLine();
                        Console.WriteLine("Last name of the student");
                        string lN = Console.ReadLine();
                        DisplayAttendancePerStudent(fN, lN);
                        break;
                    case 4:
                        DisplayStudentResults();
                        break;
                    case 5:
                        DisplayExams();
                        break;
                    case 6:
                        AddExamAssignment();
                        break;
                    case 7:
                        GiveAbsenceToStudents();
                        break;
                    case 8:
                        TakeOffAbsences();
                        break;
                    case 9:
                        ReadDispo();
                        DisplayDispo();
                        AddDispo();
                        WriteDispo();
                        DisplayDispo();
                        break;
                    case 10:
                        ReadDispo();
                        DisplayDispo();
                        RemoveDispo();
                        DisplayDispo();
                        break;
                    case 11:
                        ReadDispo();
                        DisplayDispo();
                        EditDispo();
                        DisplayDispo();
                        break;
                    case 12:
                        ReadDispo();
                        DisplayDispo();
                        break;
                    case 13:
                        Console.WriteLine("First name of the tutor");
                        fN = Console.ReadLine();
                        Console.WriteLine("Last name of the tutor");
                        lN = Console.ReadLine();
                        DisplayTutorInfo(fN, lN);
                        break;
                    case 14:
                        DisplayStudentResults();
                        MeanExam();
                        break;
                    case 15:
                        DisplayStudentClass();
                        break;
                    case 16:
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("Do you want to disconnect? if yes, enter Y else enter N");
                carryOn = Console.ReadLine();
                while (carryOn != "Y" && carryOn != "N")
                {
                    Console.WriteLine("Do you want to disconnect? if yes, enter Y else enter N");
                    carryOn = Console.ReadLine();
                }
                Console.Clear();
            }
        }

        private void DisplayStudentClass()
        {
            if (Class.Count != 0)
            {
                Console.WriteLine("Level: " + Class[0].Level);
                Console.WriteLine("Class: " + Class[0].Class);
                foreach (Student stud in Class)
                {
                    Console.WriteLine(stud.name + " " + stud.lastname);
                }
            }
            else
            {
                Console.WriteLine("There are no students in this class");
            }

        }

        public void DisplayPersonalInfos()
        {

            Console.WriteLine($"First Name: {name}\n" +
                $"Last Name: {lastname}\n" +
                $"phone number: {phone}\n" +
                $"email address: {login}\n" +
                $"subject(s) taught: {SubjectsTaught()}\n ");
        }

        public string SubjectsTaught()
        {
            string sub = null;
            for (int i = 0; i < SubjectTaught.Count; i++)
            {
                sub = sub + " " + Convert.ToString(SubjectTaught[i]);
            }
            return sub;
        }

        public void MeanExam()
        {

            for (int i = 0; i < subjectTaught.Count; i++)
            {
                Console.Write(subjectTaught[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Choose the subject");
            string subject = Console.ReadLine();
            Console.WriteLine("Date ?");
            string date = Console.ReadLine();

            double sum = 0;
            int num = 0;
            for (int i = 0; i < Class.Count; i++)
            {
                for (int j = 0; j < Class[i].Grades.Count; j++)
                {

                    if (Class[i].Grades[j].Date == date && Convert.ToString((Subject)Class[i].Grades[j].Sub) == subject)
                    {

                        sum = sum + Class[i].Grades[j].Mark;
                        num++;
                    }
                }

            }
            double mean = sum / num;
            Console.WriteLine($"The mean of this exam is {mean}");

        }

        private void DesignationOfLevelAndClass()
        {
            Class = new List<Student>();
            while (Class.Count == 0)
            {

                for (int i = 0; i < levelandclasses.Count; i++)
                {
                    Console.WriteLine("Level: " + levelandclasses[i].Level);
                    Console.Write("Classes: ");
                    for (int j = 0; j < levelandclasses[i].Classes.Count; j++)
                    {
                        Console.Write(levelandclasses[i].Classes[j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Choice of the class and level to modify");
                Console.WriteLine("Level ?");
                level2 = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("class ?");
                class2 = Convert.ToInt32(Console.ReadLine());
                for (int m = 0; m < ClassesStudent.Count; m++)
                {
                    if (ClassesStudent[m].Level == level2 && ClassesStudent[m].ClassGroup == class2)
                    {
                        for (int i = 0; i < ClassesStudent[m].ClassStudent.Count; i++)
                        {
                            Class.Add(ClassesStudent[m].ClassStudent[i]);
                        }
                    }
                }
                if (Class.Count == 0)
                {
                    Console.WriteLine("Enter another level and/or class, there is no student assigned to this class");
                }
            }
        }

        private void DisplayTutorList()
        {
            bool test = true;
            foreach (Student student in Class)
            {
                if (student.Tutor == true) Console.WriteLine(student.name + " " + student.lastname); test = false;
            }
            if (test == true) Console.WriteLine("No student in this class is a tutor");
            Console.WriteLine();
        }

        private void DisplayTutorInfo(string firstName, string lastName)
        {
            foreach (Student student in Class)
            {
                if (student.Tutor == true && student.name == firstName && student.lastname == lastName) student.DisplayPersonalInfos();
                if (student.name == firstName && student.lastname == lastName) Console.WriteLine("This student is not a tutor.");
            }
        }

        private void DisplayAttendancePerStudent(string name, string lastName)
        {
            foreach (Student stud in Class)
            {
                if (stud.name == name && stud.lastname == lastName)
                {
                    Console.WriteLine($"The student {stud.lastname} {stud.name} has been absent {stud.Absences} times");
                }

            }

        }

        private void DisplayStudentResults()
        {
            foreach (Student stud in Class)
            {
                Console.WriteLine(stud.name + " " + stud.lastname + ":");
                if (stud.Grades.Count == 0) Console.WriteLine("There are no results for this student");
                foreach (Exam exam in stud.Grades)
                {
                    Console.WriteLine($"subject:{(Subject)exam.Sub} Mark:{exam.Mark} coef:{exam.Coef} date:{exam.Date}");
                }
                Console.WriteLine();
            }


        }

        public void DisplayExams()
        {
            for (int i = 0; i < Levelandclasses.Count; i++)
            {
                for (int j = 0; j < Levelandclasses[i].Classes.Count; j++)
                {
                    Console.Write("Level " + Levelandclasses[i].Level + " ");
                    Console.WriteLine("Class " + Levelandclasses[i].Classes[j]);
                    Calendar.ReadExamAssignment(Levelandclasses[i].Classes[j], Levelandclasses[i].Level);
                    Console.WriteLine();

                }
            }
        }

        public void AddExamAssignment()
        {
            string[] EnumTab = Enum.GetNames(typeof(Subject));
            Console.Write("Choose the subject of the assignment ");
            for (int i = 0; i < EnumTab.Length; i++)
            {
                Console.Write(EnumTab[i] + " ");
            }
            Console.WriteLine();
            string subject = Console.ReadLine();
            Console.WriteLine("Year ?");
            int year = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Month ?");
            int month = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Day ?");
            int day = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Hour ?");
            int hour = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Minute ?");
            int minute = Convert.ToInt32(Console.ReadLine());
            DateTime date = new DateTime(year, month, day, hour, minute, 00);
            Console.WriteLine("Description of the exam");
            string description = Console.ReadLine();

            Calendar.AddExamAssignment(subject, date, description, class2, level2);
        }

        private string DisplayAdminInfos(Admin admin)
        {
            return admin.ToString();
        }

        public void AddResultsExam()
        {

            string[] tabEnum = Enum.GetNames(typeof(Subject));
            Exam exam = new Exam();
            //string coursesAvailable = null;
            string[] test = Enum.GetNames(typeof(Subject));
            /*
            for (int index = 0; index < test.Length; index++)
            {
                coursesAvailable = (coursesAvailable + " " + test[index]);
            }*/

            Console.WriteLine("Enter the subject : " + SubjectsTaught());
            string subjects = Console.ReadLine();
            int i = 0;
            for (; i < tabEnum.Length; i++)
            {
                if (subjects == tabEnum[i])
                {
                    exam.Sub = i;
                    break;
                }
            }

            Console.Write("Enter the coefficient : ");
            double Coef = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter the date of the exam : ");
            string Date = Console.ReadLine();

            for (int j = 0; j < Class.Count; j++)
            {
                if (CheckCourses((Subject)i, Class[j]) == true)
                {
                    exam = new Exam();
                    exam.Coef = Coef;
                    exam.Date = Date;
                    exam.Sub = i;
                    Console.Write($"Enter the mark of {Class[j].lastname} {Class[j].name} : ");
                    exam.Mark = Convert.ToDouble(Console.ReadLine());
                    Class[j].Grades.Add(exam);
                }
            }

            AddGradesToStudentFile((Subject)exam.Sub);
        }

        public void AddGradesToStudentFile(Subject sub)//modify the student list, need to be used after AddResultsExam()
        {
            List<Student> listStudent = new List<Student>();// list of the students who have grades added
            for (int i = 0; i < Class.Count; i++)
            {
                if (CheckCourses(sub, Class[i]) == true)
                {
                    Exam[] tabExam = new Exam[Class[i].Grades.Count];
                    for (int j = 0; j < Class[i].Grades.Count; j++)//add in tabexam all the exam of the student
                    {
                        tabExam[j] = Class[i].Grades[j];
                    }
                    Sort(tabExam);//sort the tabExam 

                    string grades = null;
                    // 2 cases: the student has 1 exam in his tab or he has more than one
                    if (tabExam.Length == 1)
                    {
                        //grades = Convert.ToString(tabExam[0].Sub);
                        grades = grades + tabExam[0].ToString();
                        for (int j = 0; j < Class[i].Courses.Count; j++)// add the number of the course that hasn't been modified
                        {
                            if (Class[i].Courses[j] != (Subject)tabExam[0].Sub)
                            {

                                string[] tabEnum = Enum.GetNames(typeof(Subject));// tabEnum contains all the data that can be found in the enum class but under the form of a string
                                for (int k = 0; k < tabEnum.Length; k++)
                                {
                                    if (Class[i].Courses[j] == (Subject)k)
                                    {
                                        grades = grades + ";" + k;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        grades = tabExam[0].ToString();
                        for (int k = 1; k < tabExam.Length - 1; k++)
                        {
                            // 2 possibilities: the exam is in the same subject than the next exam in the tabExam or it is not.
                            if (tabExam[k].Sub == tabExam[k + 1].Sub) grades = grades + "," + tabExam[k].ToString();
                            else grades = grades + "," + tabExam[k].ToString() + ";";

                        }
                        if (tabExam[tabExam.Length - 2].Sub == tabExam[tabExam.Length - 1].Sub) grades = grades + "," + tabExam[tabExam.Length - 1].ToString();
                        else grades = grades + tabExam[tabExam.Length - 1].ToString();


                        List<int> save = new List<int>();
                        for (int j = 0; j < Class[i].Courses.Count; j++)
                        {
                            bool check2 = true;
                            for (int l = 0; l < tabExam.Length; l++)
                            {
                                if (Class[i].Courses[j] == (Subject)tabExam[l].Sub) check2 = false;
                            }
                            if (check2 == true)
                            {
                                string[] tabEnum = Enum.GetNames(typeof(Subject));
                                for (int k = 0; k < tabEnum.Length; k++)
                                {
                                    if (Class[i].Courses[j] == (Subject)k)
                                    {
                                        if (save.Count == 0)
                                        {
                                            save.Add(k);
                                            grades = grades + ";" + k;
                                            break;
                                        }
                                        else
                                        {
                                            bool check = true;
                                            for (int m = 0; m < save.Count; m++)
                                            {
                                                if (k == save[m]) check = false;
                                            }
                                            if (check == true) grades = grades + ";" + k;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    string tutor = null;
                    if (Class[i].Tutor == false) tutor = "0";
                    else tutor = "1";
                    Class[i].Data = ($"{Class[i].name};{Class[i].lastname};{Class[i].mail};{Class[i].StudentID};{Class[i].birthDate};{Class[i].Absences};{Class[i].phone};{tutor};{Class[i].GradeLevel};{Class[i].Workgroup};{Class[i].UnpaidFees};{Class[i].NbSubject}");
                    Class[i].Data = Class[i].Data + ";" + grades;
                    listStudent.Add(Class[i]);
                }
            }

            //modification of the personal data
            // read all the student file and add the data of the student who are not in the workgroup chosen in the constructor
            StreamReader reader = new StreamReader(pathStudent);
            List<string> tab = new List<string>();
            string temp = " ";
            while (temp != null)
            {
                temp = reader.ReadLine();
                if (temp == null) break;
                string[] comparison = temp.Split(';');

                bool test = false;
                for (int i = 0; i < listStudent.Count; i++)
                {
                    if (listStudent[i].name == comparison[0] && listStudent[i].lastname == comparison[1])
                    {
                        test = true;
                    }
                }
                if (test == false)
                {
                    tab.Add(temp);
                }

            }
            reader.Close();


            for (int j = 0; j < listStudent.Count; j++)// add all the students who are in the same workgroup but who have not the subject
            {
                tab.Add(listStudent[j].Data);
            }

            File.Delete(pathStudent);// delete the file student

            FileStream stream = new FileStream(pathStudent, FileMode.OpenOrCreate);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                //keep all the data already present
                for (int i = 0; i < tab.Count; i++)
                {
                    writer.WriteLine(tab[i]);
                    //Console.WriteLine("passe2");
                }
            }
            stream.Dispose();
        }

        public static void Sort(Exam[] tab)// sort from the min to the the max
        {
            for (int i = 0; i < tab.Length - 1; i++)
            {
                int minIndex = i; int minValue = tab[i].Sub;
                for (int j = i + 1; j < tab.Length; j++)
                {
                    if (tab[j].Sub.CompareTo(minValue) < 0)
                    {
                        minIndex = j; minValue = tab[j].Sub;
                    }
                }
                Swap(tab, i, minIndex);
            }


        }

        private static void Swap(Exam[] tab, int first, int second)
        {
            Exam temp = tab[first];
            tab[first] = tab[second];
            tab[second] = temp;
        }

        public bool CheckCourses(Subject subject, Student student)// check if a student has this course in his course list
        {
            bool check = false;
            for (int i = 0; i < student.Courses.Count; i++)
            {
                if (subject == student.Courses[i])
                {
                    check = true;
                }
            }
            return check;
        }

        private void GiveAbsenceToStudents()
        {
            Console.WriteLine("who is absent ? ");
            Console.WriteLine("lastname : ");
            string forenameAbs = Console.ReadLine();
            Console.WriteLine("name : ");
            string nameAbs = Console.ReadLine();
            bool test = false;
            foreach (Student student in Class)
            {
                if (student.lastname == forenameAbs && student.name == nameAbs)
                {
                    student.Absences += 1;
                    student.ModifyDataStudent();

                    test = true;
                }
            }
            if (test == false) Console.WriteLine("the student has not been found");
        }

        private void TakeOffAbsences()
        {

            Console.WriteLine("last name : ");
            string lastNameAbs = Console.ReadLine();
            Console.WriteLine("first name : ");
            string nameAbs = Console.ReadLine();
            Console.WriteLine("Number of absences you want to take off");
            int absences = Convert.ToInt32(Console.ReadLine());
            bool test = false;
            foreach (Student student in Class)
            {
                if (student.lastname == lastNameAbs && student.name == nameAbs)
                {
                    if (student.Absences - absences > -1)
                    {
                        student.Absences = student.Absences - absences;
                        student.ModifyDataStudent();
                    }
                    else Console.WriteLine("The number of absences you want to take off is impossible.");
                    test = true;
                }
            }
            if (test == false) Console.WriteLine("the student has not been found");

        }

        private void GiveHomework()//to do
        {
            //lorsqu'on aura décidé d'une plateforme pour mettre les devoirs
        }

        public void EditStudents()
        {
            string line = "";
            foreach (Student student in Class)
            {
                line = ($"{student.lastname};{student.name};{student.StudentID};{student.birthDate};{student.Absences};{student.mail};{student.phone};{student.Tutor};{student.GradeLevel};{student.Workgroup};");


            }

        }

        public List<string>[] ReadDispo()
        {
            string disponibilities = "";
            StreamReader dispoReader = new StreamReader(PathDispo);
            string line = "";
            while (line != null)
            {
                line = dispoReader.ReadLine();
                if (line == null | line == "") break;
                if (line.Split(';')[0] == name & line.Split(';')[1] == lastname)
                {
                    disponibilities = line;
                    break;
                }
            }
            dispoReader.Close();
            for (int i = 0; i < disponibilities.Split(';').Length; i++)         //transtiping to List<string>[]
            {
                List<string> tempList = new List<string>();
                foreach (string st in disponibilities.Split(';')[i].Split(',')) tempList.Add(st);
                tempList.RemoveAll(item => item == "");         //removes the empty strings that appears if there is a day witout disponibilities
                disponibility[i] = tempList;
            }
            return disponibility;
        }

        public void AddDispo()
        {
            string test = "yes";
            while (test == "yes")
            {
                string dtest = "yes";
                Console.WriteLine("Which day are you disponible ?");
                Console.WriteLine("Monday : type 1");
                Console.WriteLine("Tuesday : type 2");
                Console.WriteLine("Wednesday : type 3");
                Console.WriteLine("Thursday : type 4");
                Console.WriteLine("Friday : type 5");
                int day = Convert.ToInt32(Console.ReadLine());
                while (dtest == "yes")
                {

                    Console.Write("Enter a time slot (ex 09:30-11:00) : ");
                    string slot = Console.ReadLine();
                    if (disponibility[day + 1].Count == 0) { disponibility[day + 1].Add(slot); }
                    else
                    {
                        for (int compt = 0; compt < disponibility[day + 1].Count; compt++)
                        {
                            if (slot.CompareTo(disponibility[day + 1][compt]) == 0)              //case if the time slot is already entered
                            {
                                Console.WriteLine("the time slot is already filled");
                                break;
                            }
                            if ((slot.CompareTo(disponibility[day + 1][compt]) < 0) | (slot.CompareTo(disponibility[day + 1][compt]) > 0 & compt == disponibility[day + 1].Count))               //case if the time entered is before another
                            {
                                disponibility[day + 1].Insert(compt, slot);
                                break;
                            }
                        }
                    }
                    Console.WriteLine("Do you want to add another disponibility this day ?");    //while loop exit
                    Console.Write("type yes or no : ");
                    dtest = Console.ReadLine();
                }
                Console.WriteLine("Do you want to add disponibilities on another day ?");       //while loop exit
                Console.Write("type yes or no : ");
                test = Console.ReadLine();
            }
        }

        public void RemoveDispo()
        {
            string test = "yes";
            while (test == "yes")
            {
                string dtest = "yes";
                Console.WriteLine("Which day are you no longer disponible ?");
                Console.WriteLine("Monday : type 1");
                Console.WriteLine("Tuesday : type 2");
                Console.WriteLine("Wednesday : type 3");
                Console.WriteLine("Thursday : type 4");
                Console.WriteLine("Friday : type 5");
                int day = Convert.ToInt32(Console.ReadLine());
                while (dtest == "yes")
                {
                    Console.Write("Enter the time slot you wish to remove (ex 09:30-11:00) : ");
                    string slot = Console.ReadLine();
                    if (disponibility[day + 1].Contains(slot)) disponibility[day + 1].Remove(slot); //remove an instance of the slot entered if it exists
                    else Console.WriteLine("No matching time slot found this day.");
                    Console.WriteLine("Do you want to remove another disponibility this day ?");    //while loop exit
                    Console.Write("type yes or no : ");
                    dtest = Console.ReadLine();
                }
                Console.WriteLine("Do you want to remove disponibilities on another day ?");       //while loop exit
                Console.Write("type yes or no : ");
                test = Console.ReadLine();
            }
        }

        public void DisplayDispo()
        {
            string days = "Monday          Tuesday         Wednesday       Thursday        Friday";
            string space1 = "           ";
            string space2 = "     ";
            string line = "";
            //Console.Write(disponibility[0][0]);
            //Console.WriteLine(disponibility[1][0]);
            Console.WriteLine(days);
            for (int i = 0; i < 8; i++)
            {
                line = "";
                for (int j = 2; j < 7; j++)
                {
                    if (disponibility[j].Count >= i + 1) line += disponibility[j][i] + space2;
                    else line += space1 + space2;
                }
                Console.WriteLine(line);
            }
        }

        public void WriteDispo()
        {
            string disponibilities = name + ";" + lastname;
            for (int i = 1; i < 6; i++)         //transtype to string
            {

                disponibilities += ";";
                if (disponibility[i + 1].Count != 0)
                {
                    disponibilities += disponibility[i + 1][0];
                    foreach (string slots in disponibility[i + 1].Skip(1))
                    {
                        disponibilities += ",";
                        disponibilities += slots;
                    }
                }
            }
            StreamReader streamReader = new StreamReader(PathDispo);
            List<string> lines = new List<string>();
            string line = "";
            while (line != null)
            {
                line = streamReader.ReadLine();
                if (line == null) break;
                lines.Add(line);
            }
            streamReader.Close();
            bool check = true;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains($"{name};{lastname}") == true)
                {
                    lines[i] = disponibilities;
                    check = false;
                }
            }
            if (check == true)
            {
                Console.WriteLine("passe2");
                Console.WriteLine(disponibilities);
                lines.Add(disponibilities);
            }
            File.Delete(PathDispo);
            FileStream stream = new FileStream(PathDispo, FileMode.OpenOrCreate);
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                foreach (string l in lines)
                {
                    streamWriter.WriteLine(l);
                }
            }
            stream.Dispose();
        }

        public void EditDispo()
        {
            disponibility = ReadDispo();

            foreach (List<string> l in disponibility)
            {
                foreach (string s in l) Console.Write(s);
            }

            string test1 = "";
            while (test1 != "3")
            {
                Console.WriteLine("What do you want to do ?");
                Console.WriteLine("1 : Add a disponibility");
                Console.WriteLine("2 : Remove a disponibility");
                Console.WriteLine("3 : Nothing");
                test1 = Console.ReadLine();
                if (test1 == "3") break;
                if (test1 == "1")
                {
                    DisplayDispo();
                    AddDispo();
                }
                if (test1 == "2")
                {
                    DisplayDispo();
                    RemoveDispo();
                }
            }
            WriteDispo();
        }




    }
}
