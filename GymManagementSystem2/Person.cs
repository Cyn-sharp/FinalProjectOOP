using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem2
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public int Id { get; set; }

        private static List<int> usedIds = new List<int>();


        public Person(string name, int age, string sex)
        {
            Name = name;
            Age = age;
            Sex = sex;
            Id = GenerateUniqueId();
        }

        private int GenerateUniqueId()
        {
            Random rand = new Random();
            int newId;

            // Ensure the ID is unique
            do
            {
                newId = rand.Next(1000, 10000); // Generates a 4-digit number
            } while (usedIds.Contains(newId));

            usedIds.Add(newId); // Store the new unique ID
            return newId;
        }
    }

    public class Member : Person
    {
        public double Weight { get; set; }
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
        public Instructor AssignedInstructor { get; set; }
        public Payment Payment { get; set; }
        public List<WeightRecord> WeightHistory { get; set; } = new List<WeightRecord>();
        public bool IsActive { get; set; }


        public Member(string name, int age, string sex, double weight) : base(name, age, sex)
        {
            Weight = weight;
            WeightHistory.Add(new WeightRecord(weight));
            IsActive = false;
            AssignExercises();
        }
        
        public void UpdateWeight(double newWeight)
        {
            WeightHistory.Add(new WeightRecord(newWeight));
            Weight = newWeight;
        }

        public void DisplayWeightHistory()
        {
            if (WeightHistory.Count == 0)
            {
                Console.WriteLine("No weight history available.");
                return;
            }

            Console.WriteLine("\n--- Weight History ---");
            for (int i = 0; i < WeightHistory.Count; i++)
            {
                var history = WeightHistory[i];
                Console.WriteLine($"Date: {history.Date.ToShortDateString()}, Weight: {history.Weight} kg");

                // Display progress every 4 records
                if ((i + 1) % 4 == 0)
                {
                    PrintProgress(WeightHistory.GetRange(i - 3, 4)); // Get the last 4 records
                }
            }
            Console.WriteLine(); // For extra spacing after displaying history
        }

        // Method to print progress for the last few weight records
        private void PrintProgress(List<WeightRecord> recentRecords)
        {
            if (recentRecords.Count >= 4)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                double weightChange = recentRecords.Last().Weight - recentRecords.First().Weight;
                Console.WriteLine($"\nProgress Update (Last 4 Records):");
                Console.WriteLine($"Weight change: {weightChange} kg");

                if (weightChange > 0)
                    Console.WriteLine("You've gained weight!");
                else if (weightChange < 0)
                    Console.WriteLine("You've lost weight!");
                else
                    Console.WriteLine("No significant weight change.");
            }
        }

        public void Login()
        {
            IsActive = true;
            Console.WriteLine($"{Name} is now logged in and active.");
        }
        public void Logout()
        {
            IsActive = false;
            Console.WriteLine($"{Name} is now logged out and inactive.");
        }

        public class WeightRecord
        {
            public double Weight { get; set; }
            public DateTime Date { get; set; }

            public WeightRecord(double weight)
            {
                Weight = weight;
                Date = DateTime.Now; // The current date when the weight is updated
            }
        }


        public void AssignExercises()
        {
           
            Exercises.Clear();

            if (Age < 30 && Sex == "Male" && Weight <= 82)
            {
                Console.WriteLine("Workout Routine (3–4 Days Per Week)");

               
                Console.WriteLine("Day 1 - Upper Body Strength");
                Exercises.Add(new Exercise("Bench Press (Barbell/Dumbbell)", 4, 10));
                Exercises.Add(new Exercise("Pull-Ups/Assisted Pull-Ups", 4, 10));
                Exercises.Add(new Exercise("Overhead Press (Barbell/Dumbbell)", 3, 12));

                
                Console.WriteLine("Day 2 - Back & Shoulders");
                Exercises.Add(new Exercise("Barbell Rows", 4, 8));
                Exercises.Add(new Exercise("Push-Ups (Weighted if advanced)", 4, 10));

                
                Console.WriteLine("Day 3 - Lower Body & Core");
                Exercises.Add(new Exercise("Squats", 4, 10));
                Exercises.Add(new Exercise("Lunges", 4, 8));

            }
            else if (Age < 30 && Sex == "Male" && Weight >= 82)
            {
                Console.WriteLine("Workout Routine (3–5 Days Per Week)");

                
                Console.WriteLine("Day 1 - Upper Body Strength");
                Exercises.Add(new Exercise("Flat Barbell Bench Press", 4, 8));
                Exercises.Add(new Exercise("Pull-Ups/Chin-Ups (Assisted if Needed)", 4, 10));

                
                Console.WriteLine("Day 2 - Push Exercises");
                Exercises.Add(new Exercise("Incline Dumbbell Press", 3, 10));
                Exercises.Add(new Exercise("Dips", 3, 8));

                
                Console.WriteLine("Day 3 - Back & Shoulders");
                Exercises.Add(new Exercise("Bent-Over Barbell Rows", 4, 8));
                Exercises.Add(new Exercise("Face Pulls (Cable)", 3, 15));

                Console.WriteLine("Day 4 - Legs & Core");
                Exercises.Add(new Exercise("Leg Press", 4, 8));
                Exercises.Add(new Exercise("Ab Rollouts", 3, 10));

            }
            else if (Age < 30 && Sex == "Female" && Weight >= 82)
            {
                Console.WriteLine("Workout Routine (3–5 Days Per Week)");

                Console.WriteLine("Day 1 - Lower Body Strength");
                Exercises.Add(new Exercise("Barbell Squats", 4, 8));
                Exercises.Add(new Exercise("Hip Thrusts (Barbell or Dumbbell)", 4, 10));

                Console.WriteLine("Day 2 - Posterior Chain & Core");
                Exercises.Add(new Exercise("Romanian Deadlifts", 3, 10));
                Exercises.Add(new Exercise("Lunges (Bodyweight or Weighted)", 4, 8));


                Console.WriteLine("Day 3 - Glutes & Legs");
                Exercises.Add(new Exercise("Standing Calf Raises", 3, 15));
                Exercises.Add(new Exercise("Leg Curls", 4, 10));

                Console.WriteLine("Day 4 - Full Body or Core");
                Exercises.Add(new Exercise("Planks", 4, 10));
                Exercises.Add(new Exercise("Russian Twists", 3, 12));
            }
            else if (Age < 30 && Sex == "Female" && Weight <= 82)
            {
                Console.WriteLine("Workout Routine (3–5 Days Per Week)");

               
                Console.WriteLine("Day 1 - Core & Conditioning");
                Exercises.Add(new Exercise("Plank Variations", 4, 8));
                Exercises.Add(new Exercise("Mountain Climbers", 4, 10));

           
                Console.WriteLine("Day 2 - Full Body Workout");
                Exercises.Add(new Exercise("Russian Twists (WEIGHTED)", 3, 10));
                Exercises.Add(new Exercise("Lunges (Bodyweight or Weighted)", 4, 8));

                Console.WriteLine("Day 3 - Glutes & Conditioning");
                Exercises.Add(new Exercise("Kettlebell Swings", 3, 15));
                Exercises.Add(new Exercise("Step-Ups", 4, 10));

               
                Console.WriteLine("Day 4 - Mobility & Flexibility");
                Exercises.Add(new Exercise("Yoga", 3, 8));
                Exercises.Add(new Exercise("Stretching", 3, 8));
            }
            else
            {
  
                Console.WriteLine("Workout Routine (3–4 Days Per Week)");


                Console.WriteLine("Day 1 - Full Body Conditioning");
                Exercises.Add(new Exercise("Walking", 3, 10));
                Exercises.Add(new Exercise("Stretching", 3, 8));

                Console.WriteLine("Day 2 - Full Body Conditioning");
                Exercises.Add(new Exercise("Yoga", 3, 8));
                Exercises.Add(new Exercise("Jogging", 3, 8));

                Console.WriteLine("Day 3 - Core & Flexibility");
                Exercises.Add(new Exercise("Core Workouts", 3, 10));
                Exercises.Add(new Exercise("Pilates", 3, 8));
            }
        }
    }

    public class Instructor : Person
    {
        public string Specialty { get; set; }
        public List<Member> AssignedMembers { get; set; } = new List<Member>();

        public Instructor(string name, int age, string sex, string specialty) : base(name, age, sex)
        {
            Specialty = specialty;
        }

        public void AddMember(Member member)
    {
        if (!AssignedMembers.Contains(member))
        {
            AssignedMembers.Add(member);
        }
    }

    public void RemoveMember(Member member)
    {
        if (AssignedMembers.Contains(member))
        {
            AssignedMembers.Remove(member);
        }
    }
    }

    public class Exercise
    {
        public string Name { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }

        public Exercise(string name, int sets, int reps)
        {
            Name = name;
            Sets = sets;
            Reps = reps;
        }
    }

    public class WeightHistory
    {
        public DateTime Date { get; set; }
        public double Weight { get; set; }

        public WeightHistory(double weight)
        {
            Date = DateTime.Now;  // Sets the current date
            Weight = weight;
        }
    }
}

