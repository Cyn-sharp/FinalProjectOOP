using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem2
{

    public interface IGymActions
    {
        void AddMember(Member member);
        void AddInstructor(Instructor instructor);
        void DisplayMembers();
        void DisplayInstructors();
    }


    public class GymController : IGymActions
    {
        private List<Member> members = new List<Member>();
        private List<Instructor> instructors = new List<Instructor>();

        public void AddMember(Member member)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Available Instructors:");
            DisplayInstructors();
            Console.Write("Please enter the number of the instructor you want to assign (or 0 to skip): ");


            int instructorChoice;
            if (int.TryParse(Console.ReadLine(), out instructorChoice) && instructorChoice > 0 && instructorChoice <= instructors.Count)
            {
                member.AssignedInstructor = instructors[instructorChoice - 1];
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No instructor assigned.");
            }

            members.Add(member);
            FileHandler.SaveMembers(members);
            Console.WriteLine("Member added successfully.");
            Console.ResetColor();
        }

        public void RemoveMember()
        {
            Console.Write("Enter the name of the member you want to remove: ");
            string memberName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(memberName))
            {
                Console.WriteLine("Error: Member name cannot be empty.");
            }
            else
            {
                Member memberToRemove = null;

                foreach (Member member in members)
                {
                    if (member.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase))
                    {
                        memberToRemove = member;
                        break;
                    }
                }

                if (memberToRemove != null)
                {
                    members.Remove(memberToRemove);

                    try
                    {
                        FileHandler.SaveMembers(members);
                        Console.WriteLine($"Member '{memberName}' removed successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving members to file: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine($"Member '{memberName}' not found.");
                }
            }
        }
        public void SearchMember()
        {
            Console.Write("Enter the name of the member you want to search for: ");
            string memberName = Console.ReadLine();

            // Find members whose names match the search query
            var foundMembers = members.Where(m => m.Name.IndexOf(memberName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (foundMembers.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No members found with the name '{memberName}'.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                // Table header
                string header = "+------+--------------------+-----------+--------------------+--------------------+";
                string titles = "| Id   | Name               | Weight    | Instructor         | Membership Type    |";
                int consoleWidth = Console.WindowWidth;
                int padding = (consoleWidth - header.Length) / 2;

                Console.WriteLine(new string(' ', padding) + header);
                Console.WriteLine(new string(' ', padding) + titles);
                Console.WriteLine(new string(' ', padding) + header);
                Console.ResetColor();

                // Table content for found members
                foreach (var member in foundMembers)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    string instructorName = member.AssignedInstructor != null ? member.AssignedInstructor.Name : "None";
                    string membershipType = member.Payment != null ? member.Payment.MembershipType.ToString() : "None";

                    string row = $"| {member.Id,-4} | {member.Name,-18} | {member.Weight,-9} | {instructorName,-18} | {membershipType,-18} |";
                    Console.WriteLine(new string(' ', padding) + row);

                    Console.ResetColor();
                }

                // Table footer
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(new string(' ', padding) + header);
                Console.ResetColor();
            }

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }


        public void AddInstructor(Instructor instructor)
        {
            instructors.Add(instructor);
            FileHandler.SaveInstructors(instructors);
            Console.WriteLine("Instructor added successfully.");
        }
        public void SearchInstructor()
        {
            Console.Write("Enter the name of the instructor you want to search for: ");
            string instructorName = Console.ReadLine();

            var foundInstructors = instructors.Where(i => i.Name.IndexOf(instructorName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (foundInstructors.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No instructors found with the name '{instructorName}'.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("---- Search Results ----");
                Console.WriteLine("+------+--------------------+-----+-------+--------------------+");
                Console.WriteLine("| Id   | Name               | Age | Sex   | Specialty          |");
                Console.WriteLine("+------+--------------------+-----+-------+--------------------+");

                foreach (var instructor in foundInstructors)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"| {instructor.Id,-4} | {instructor.Name,-18} | {instructor.Age,-3} | {instructor.Sex,-5} | {instructor.Specialty,-18} |");
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("+------+--------------------+-----+-------+--------------------+");

                Console.ResetColor();
            }

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }


        private Instructor AssignInstructor(Member member)
        {
            Instructor bestMatch = null;
            int closestAgeDifference = int.MaxValue;

            foreach (var instructor in instructors)
            {
                if (instructor.Specialty == member.Exercises[0].Name && instructor.Sex == member.Sex)
                {
                    int ageDifference = Math.Abs(instructor.Age - member.Age);

                    if (ageDifference < closestAgeDifference)
                    {
                        closestAgeDifference = ageDifference;
                        bestMatch = instructor;
                    }
                }
            }
            return bestMatch;
        }
        public void DisplayMembers()
        {
            if (members.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No members to display.");
                Console.ResetColor();
                return;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+------+--------------------+-----------+--------------------+--------------------+----------------+");
            Console.WriteLine("| Id   | Name               | Weight    | Instructor         | Membership Type    | Status         |");
            Console.WriteLine("+------+--------------------+-----------+--------------------+--------------------+----------------+");
            Console.ResetColor();

            foreach (var member in members)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                string instructorName = member.AssignedInstructor != null ? member.AssignedInstructor.Name : "None";
                string membershipType = member.Payment != null ? member.Payment.MembershipType.ToString() : "None";

                // Set color for status: green for active, red for inactive
                Console.ForegroundColor = member.IsActive ? ConsoleColor.Green : ConsoleColor.Red;
                string status = member.IsActive ? "Active" : "Inactive"; // Add status based on IsActive

                // Print member details with colored status
                Console.WriteLine($"| {member.Id,-4} | {member.Name,-18} | {member.Weight,-9} | {instructorName,-18} | {membershipType,-18} | {status,-14} |");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+------+--------------------+-----------+--------------------+--------------------+----------------+");
            Console.ResetColor();

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        public void DisplayInstructors()
        {
            if (instructors.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No instructors to display.");
                Console.ResetColor();
                return;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("---- List of Instructors ----");
            Console.WriteLine("+------+-------+--------------------+--------------------+-----------------------------+");
            Console.WriteLine("| No.  | Id    | Name               | Specialty          | Members Enrolled           |");
            Console.WriteLine("+------+-------+--------------------+--------------------+-----------------------------+");

            for (int i = 0; i < instructors.Count; i++)
            {
                var instructor = instructors[i];
                var memberNames = instructor.AssignedMembers != null && instructor.AssignedMembers.Count > 0
                    ? string.Join(", ", instructor.AssignedMembers.Select(m => m.Name))
                    : "None";

                // Truncate the member names list if it's too long to fit
                if (memberNames.Length > 27)
                    memberNames = memberNames.Substring(0, 24) + "...";

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"| {i + 1,-4} | {instructor.Id,-5} | {instructor.Name,-18} | {instructor.Specialty,-18} | {memberNames,-27} |");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+------+-------+--------------------+--------------------+-----------------------------+");

            Console.ResetColor();
        }

        public void LoadExistingData()
        {
            members = FileHandler.LoadMembers();
            instructors = FileHandler.LoadInstructors();
            Console.WriteLine("Data loaded successfully.");
        }


        public void UpdateMember()
        {
            try
            {
                Console.Write("Enter the name of the member you want to update: ");
                string memberName = Console.ReadLine();


                Member memberToUpdate = members.Find(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));


                if (memberToUpdate != null)
                {
                    Console.Write("Enter the new details for the member: ");

                    Console.Write("Name: ");
                    memberToUpdate.Name = Console.ReadLine();

                    Console.Write("Age: ");
                    memberToUpdate.Age = int.Parse(Console.ReadLine());

                    Console.Write("Sex: ");
                    memberToUpdate.Sex = Console.ReadLine();

                    Console.Write("Weight: ");
                    memberToUpdate.Weight = double.Parse(Console.ReadLine());

                    FileHandler.SaveMembers(members);
                    Console.Write("Member Updated Succesfully. ");
                }
                else
                {
                    Console.WriteLine("Member not found. ");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteMember()
        {
            Console.Write("Enter the name of the member you want  to delete: ");
            string memberName = Console.ReadLine();

            Member memberToDelete = members.Find(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));

            try
            {
                if (memberToDelete != null)
                {
                    members.Remove(memberToDelete);
                    FileHandler.SaveMembers(members);
                    Console.WriteLine("Member deleted Succesfully.");
                }
                else
                {
                    Console.WriteLine("Member not found. ");
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void UpdateInstructor()
        {
            Console.Write("Enter the name of the instructor you want to update: ");
            string instructorName = Console.ReadLine();

            Instructor instructorToUpdate = instructors.Find(i => i.Name.Equals(instructorName, StringComparison.OrdinalIgnoreCase));

            if (instructorToUpdate != null)
            {
                Console.WriteLine("Enter new details for the instructor:");

                Console.Write("Name: ");
                instructorToUpdate.Name = Console.ReadLine();

                Console.Write("Age: ");
                instructorToUpdate.Age = int.Parse(Console.ReadLine());

                Console.Write("Sex (Male/Female): ");
                instructorToUpdate.Sex = Console.ReadLine();

                Console.Write("Specialty: ");
                instructorToUpdate.Specialty = Console.ReadLine();

                FileHandler.SaveInstructors(instructors);
                Console.WriteLine("Instructor updated successfully.");
            }
            else
            {
                Console.WriteLine("Instructor not found.");
            }
        }

        public void DeleteInstructor()
        {
            Console.Write("Enter the name of the instructor you want to delete: ");
            string instructorName = Console.ReadLine();

            Instructor instructorToDelete = instructors.Find(i => i.Name.Equals(instructorName, StringComparison.OrdinalIgnoreCase));

            if (instructorToDelete != null)
            {
                instructors.Remove(instructorToDelete);
                FileHandler.SaveInstructors(instructors);
                Console.WriteLine("Instructor deleted successfully.");
            }
            else
            {
                Console.WriteLine("Instructor not found.");
            }
        }

        public void PrintReceipt(Member member, Payment payment)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleHelper.WriteCentered("---- Gym Membership Receipt -----");
            ConsoleHelper.WriteCentered("---------------------------------");
            ConsoleHelper.WriteCentered($"Member Name: {member.Name}");
            ConsoleHelper.WriteCentered($"Member ID: {member.Id}");
            ConsoleHelper.WriteCentered($"Age: {member.Age}");
            ConsoleHelper.WriteCentered($"Sex: {member.Sex}");
            ConsoleHelper.WriteCentered($"Weight: {member.Weight} kg");
            ConsoleHelper.WriteCentered("---------------------------------");

            if (member.AssignedInstructor != null)
            {
                ConsoleHelper.WriteCentered($"Instructor: {member.AssignedInstructor.Name} - {member.AssignedInstructor.Specialty}");
            }
            else
            {
                ConsoleHelper.WriteCentered("Instructor: Not Assigned");
            }

            ConsoleHelper.WriteCentered("---------------------------------");
            ConsoleHelper.WriteCentered($"Membership Type: {payment.MembershipType}");
            ConsoleHelper.WriteCentered($"Payment Method: {GetPaymentMethodText(payment.PaymentMethod)}");
            ConsoleHelper.WriteCentered($"Payment Amount: {payment.Amount} pesos");
            ConsoleHelper.WriteCentered($"Payment Date: {payment.PaymentDate.ToShortDateString()}");
            ConsoleHelper.WriteCentered($"Due Date: {payment.DueDate.ToShortDateString()}");
            ConsoleHelper.WriteCentered("---------------------------------");
            ConsoleHelper.WriteCentered("Thank you for choosing our gym!");
            ConsoleHelper.WriteCentered("We wish you a great fitness journey!");
            ConsoleHelper.WriteCentered("---------------------------------");
            ConsoleHelper.WriteCentered("Please keep this receipt for your records.");
            Console.ResetColor();
        }

        private string GetPaymentMethodText(string paymentMethod)
        {
            switch (paymentMethod)
            {
                case "1":
                    return "Cash";
                case "2":
                    return "Credit Card";
                case "3":
                    return "Debit Card";
                case "4":
                    return "Bank Transfer";
                default:
                    return "Unknown Payment Method";
            }
        }

        public void DisplayAllMemberShipInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("---- All Members Information ----");

            Console.WriteLine("+-------------------+--------------------+--------------------+--------------------+--------------------+");
            Console.WriteLine("| Member ID         | Name               | Join Date          | Due Date           | Membership Type    |");
            Console.WriteLine("+-------------------+--------------------+--------------------+--------------------+--------------------+");

            foreach (var member in members)
            {
                if (member.Payment != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(
                        $"| {member.Id,-17} | {member.Name,-18} | {member.Payment.PaymentDate.ToShortDateString(),-18} | {member.Payment.DueDate.ToShortDateString(),-18} | {member.Payment.MembershipType,-18} |");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        $"| {member.Id,-17} | {member.Name,-18} | {"N/A",-18} | {"N/A",-18} | {"N/A",-18} |");
                }
            }

            Console.ResetColor();
            Console.WriteLine("+-------------------+--------------------+--------------------+--------------------+--------------------+");

            Console.ResetColor();
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }


        public bool MemberExists(int memberId)
        {
            return members.Any(m => m.Id == memberId);  // Check if any member has the given Id
        }
        public List<Member> GetAllMembers()
        {
            return members;  // Assuming 'members' is your list of Member objects
        }
        public Member GetMemberById(int memberId)
        {
            return members.FirstOrDefault(m => m.Id == memberId);  // Find the member by their ID
        }


        public void DisplayExercisesForMember(Member member)
        {
            // Clear the console and display exercises
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            // Display the header for the workout routine
            Console.WriteLine("---- Workout Routine for " + member.Name + " ----");
            Console.WriteLine("Based on the information provided, the following exercises have been assigned:\n");

            int dayCount = 1;
            int exercisesPerDay = 5;  // You can adjust how many exercises per day you want
            int exerciseCount = 0;

            // Loop through each exercise and group by day
            foreach (var exercise in member.Exercises)
            {
                // Check if we need to start a new day after the designated number of exercises
                if (exerciseCount % exercisesPerDay == 0 && exerciseCount != 0)
                {
                    dayCount++;
                    Console.WriteLine(); // Add a blank line between days
                }

                // Display the exercise under the appropriate day
                if (exerciseCount % exercisesPerDay == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Day {dayCount}:");
                }

                // Display the exercise with sets and reps
                Console.WriteLine($"  - {exercise.Name} - Sets: {exercise.Sets}, Reps: {exercise.Reps}");

                exerciseCount++;
            }

            // Optionally, you could add a space before returning to the menu
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();

            Console.ResetColor();
        }

        public Member GetMemberByName(string memberName)
        {
            return members.FirstOrDefault(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
        }

    }
}
