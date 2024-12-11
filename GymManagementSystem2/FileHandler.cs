using System;
using System.Collections.Generic;
using System.IO;

namespace GymManagementSystem2
{
    public static class FileHandler
    {
        private static string baseDirectory = @"C:\Users\CYNDRICK\Desktop\GymManagementSystem2\files";

        static FileHandler()
        {
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }
        }

        private static string membersFilePath = Path.Combine(baseDirectory, "members.txt");
        private static string instructorsFilePath = Path.Combine(baseDirectory, "instructors.txt");

        public static void SaveMembers(List<Member> members)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(membersFilePath, append: true))
                {
                    foreach (var member in members)
                    {
                        writer.WriteLine($"{member.Id},{member.Name},{member.Age},{member.Sex},{member.Weight},{member.IsActive}");
                    }
                }
                Console.WriteLine("Members data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving members data: {ex.Message}");
            }
        }

        public static List<Member> LoadMembers()
        {
            List<Member> members = new List<Member>();
            try
            {
                if (File.Exists(membersFilePath))
                {
                    using (StreamReader reader = new StreamReader(membersFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');

                            int id = int.Parse(parts[0]);
                            string name = parts[1];
                            int age = int.Parse(parts[2]);
                            string sex = parts[3];
                            double weight = double.Parse(parts[4]);
                            bool isActive = bool.Parse(parts[5]);

                            members.Add(new Member(name, age, sex, weight)
                            {
                                Id = id,
                                IsActive = isActive
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading members data: {ex.Message}");
            }
            return members;
        }

        public static void SaveInstructors(List<Instructor> instructors)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(instructorsFilePath))
                {
                    foreach (var instructor in instructors)
                    {
                        writer.WriteLine($"{instructor.Id},{instructor.Name},{instructor.Age},{instructor.Sex},{instructor.Specialty}");
                    }
                }
                Console.WriteLine("Instructors data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving instructors data: {ex.Message}");
            }
        }

        public static List<Instructor> LoadInstructors()
        {
            List<Instructor> instructors = new List<Instructor>();
            try
            {
                if (File.Exists(instructorsFilePath))
                {
                    using (StreamReader reader = new StreamReader(instructorsFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');

                            int id = int.Parse(parts[0]);
                            string name = parts[1];
                            int age = int.Parse(parts[2]);
                            string sex = parts[3];
                            string specialty = parts[4];

                            instructors.Add(new Instructor(name, age, sex, specialty)
                            {
                                Id = id
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading instructors data: {ex.Message}");
            }
            return instructors;
        }

        public static void SaveWeightHistory(Member member)
        {
            try
            {
                string weightHistoryFilePath = Path.Combine(baseDirectory, $"weight_history_{member.Id}.txt");
                using (StreamWriter writer = new StreamWriter(weightHistoryFilePath))
                {
                    foreach (var record in member.WeightHistory)
                    {
                        writer.WriteLine($"{record.Date.ToShortDateString()},{record.Weight}");
                    }
                }
                Console.WriteLine($"Weight history for member {member.Name} saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving weight history: {ex.Message}");
            }
        }

        public static List<Member.WeightRecord> LoadWeightHistory(int memberId)
        {
            List<Member.WeightRecord> weightHistory = new List<Member.WeightRecord>();
            try
            {
                string weightHistoryFilePath = Path.Combine(baseDirectory, $"weight_history_{memberId}.txt");
                if (File.Exists(weightHistoryFilePath))
                {
                    using (StreamReader reader = new StreamReader(weightHistoryFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');

                            DateTime date = DateTime.Parse(parts[0]);
                            double weight = double.Parse(parts[1]);

                            weightHistory.Add(new Member.WeightRecord(weight) { Date = date });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading weight history: {ex.Message}");
            }
            return weightHistory;
        }
    }
}
