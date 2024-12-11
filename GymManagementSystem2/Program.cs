using System;
using System.Collections.Generic;
using System.IO;

namespace GymManagementSystem2
{

    public class Program
    {
        private List<Member> members = new List<Member>();
        private List<Instructor> instructors = new List<Instructor>();

        static void Main()
        {
            GymController gymController = new GymController();


            bool running = true;

            gymController.LoadExistingData();

            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("█▀▀ █▄█ █▀▄▀█   █▀▄▀█ ▄▀█ █▄░█ ▄▀█ █▀▀ █▀▀ █▀▄▀█ █▀▀ █▄░█ ▀█▀   █▀ █▄█ █▀ ▀█▀ █▀▀ █▀▄▀█");
                Console.WriteLine("█▄█ ░█░ █░▀░█   █░▀░█ █▀█ █░▀█ █▀█ █▄█ ██▄ █░▀░█ ██▄ █░▀█ ░█░   ▄█ ░█░ ▄█ ░█░ ██▄ █░▀░█");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. Manage Members");
                Console.WriteLine("2. Manage Instructors");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                try
                {
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            ManageMembers(gymController);
                            break;
                        case 2:
                            ManageInstructors(gymController);
                            break;
                        case 0:
                            running = false;
                            Console.Clear();
                            Console.WriteLine("▀█▀ █░█ ▄▀█ █▄░█ █▄▀   █▄█ █▀█ █░█   █▀▀ █▀█ █▀█   █░█ █▀ █ █▄░█ █▀▀");
                            Console.WriteLine("░█░ █▀█ █▀█ █░▀█ █░█   ░█░ █▄█ █▄█   █▀░ █▄█ █▀▄   █▄█ ▄█ █ █░▀█ █▄█");
                            break;
                        default:
                            Console.WriteLine("Please enter a valid number.");
                            break;
                    }

                    if (running)
                    {
                        Console.WriteLine("\nPress Enter to return to the main menu...");
                        Console.ReadLine();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void ManageMembers(GymController gymController)
        {
            List<Member> members = new List<Member>();
            bool managingMembers = true;
            while (managingMembers)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("█▀▄▀█ ▄▀█ █▄░█ ▄▀█ █▀▀ █▀▀   █▀▄▀█ █▀▀ █▀▄▀█ █▄▄ █▀▀ █▀█ █▀");
                Console.WriteLine("█░▀░█ █▀█ █░▀█ █▀█ █▄█ ██▄   █░▀░█ ██▄ █░▀░█ █▄█ ██▄ █▀▄ ▄█");
                Console.WriteLine("");
                Console.WriteLine("1. Add Member");
                Console.WriteLine("2. Display Members");
                Console.WriteLine("3. Update Member");
                Console.WriteLine("4. Delete Member");
                Console.WriteLine("5. Search Member");
                Console.WriteLine("6. Display Membership Info");
                Console.WriteLine("7. Update Weight");
                Console.WriteLine("8. View Weight History");
                Console.WriteLine("9. Change Member Status");
                Console.WriteLine("10. Display Member Assigned Exercises.");
                Console.WriteLine("0. Back to Main Menu");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Select an option: ");

                try
                {
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            AddMember(gymController);
                            break;
                        case 2:
                            Console.Clear();
                            gymController.DisplayMembers();
                            break;
                        case 3:
                            Console.Clear();
                            gymController.UpdateMember();
                            break;
                        case 4:
                            Console.Clear();
                            gymController.DeleteMember();
                            break;
                        case 5:
                            Console.Clear();
                            gymController.SearchMember();
                            break;
                        case 6:
                            Console.Clear();
                            gymController.DisplayAllMemberShipInfo();
                            break;
                        case 7:
                            Console.Clear();
                            UpdateMemberWeight(gymController);
                            break;
                        case 8:
                            Console.Clear();
                            ViewWeightHistory(gymController);
                            break;
                        case 9:
                            Console.Clear();
                            ChangeMemberStatus(gymController);
                            break;
                        case 10:
                            Console.Clear();
                            Console.Write("Enter the name of the member to view their exercises: ");
                            string memberName = Console.ReadLine();

                            var member = gymController.GetMemberByName(memberName);
                            if (member == null)
                            {
                                Console.WriteLine("Member not found.");
                            }
                            else
                            {
                                gymController.DisplayExercisesForMember(member);  // Call method with the member
                            }
                            break;
                        case 0:
                            managingMembers = false;
                            break;
                        default:
                            Console.WriteLine("Please enter a valid number.");
                            break;
                    }

                    if (managingMembers)
                    {
                        Console.WriteLine("\nPress Enter to return to the Members menu...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        public static void ViewWeightHistory(GymController gymController)
        {
            Console.Write("Enter the name of the member to view weight history: ");
            string memberName = Console.ReadLine();

            var member = gymController.GetMemberByName(memberName);  // Assuming GetMemberByName is implemented in GymController
            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return;
            }

            member.DisplayWeightHistory(); // Display the weight history for the selected member
        }

        public static void UpdateMemberWeight(GymController gymController)
        {
            bool continueUpdating = true;

            while (continueUpdating)
            {
                Console.Write("Enter the name of the member to update weight: ");
                string memberName = Console.ReadLine();

                var member = gymController.GetMemberByName(memberName);  // Assuming GetMemberById is implemented in GymController
                if (member == null)
                {
                    Console.WriteLine("Member not found.");
                    return;
                }

                Console.Write("Enter the new weight: ");
                double newWeight;
                while (!double.TryParse(Console.ReadLine(), out newWeight) || newWeight <= 0)
                {
                    Console.Write("Invalid weight. Please enter a valid positive number: ");
                }

                member.UpdateWeight(newWeight); // Update the member's weight and record the history
                Console.WriteLine("Weight updated successfully.");

                // Ask if the user wants to update another member
                Console.Write("Do you want to update another member's weight? (yes/no): ");
                string userResponse = Console.ReadLine().ToLower();

                if (userResponse != "yes")
                {
                    continueUpdating = false; // Exit the loop if user doesn't want to continue
                }
            }
        }


        static void ManageInstructors(GymController gymController)
        {
            bool managingInstructors = true;
            while (managingInstructors)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("█▀▄▀█ ▄▀█ █▄░█ ▄▀█ █▀▀ █▀▀   █ █▄░█ █▀ ▀█▀ █▀█ █░█ █▀▀ ▀█▀ █▀█ █▀█ █▀");
                Console.WriteLine("█░▀░█ █▀█ █░▀█ █▀█ █▄█ ██▄   █ █░▀█ ▄█ ░█░ █▀▄ █▄█ █▄▄ ░█░ █▄█ █▀▄ ▄█");
                Console.WriteLine("");
                Console.WriteLine("=== Manage Instructors ===");
                Console.WriteLine("1. Add Instructor");
                Console.WriteLine("2. Display Instructors");
                Console.WriteLine("3. Update Instructor");
                Console.WriteLine("4. Delete Instructor");
                Console.WriteLine("5. Search Instructor");
                Console.WriteLine("0. Back to Main Menu");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Select an option: ");

                try
                {
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            AddInstructor(gymController);
                            break;
                        case 2:
                            Console.Clear();
                            gymController.DisplayInstructors();
                            break;
                        case 3:
                            Console.Clear();
                            gymController.UpdateInstructor();
                            break;
                        case 4:
                            Console.Clear();
                            gymController.DeleteInstructor();
                            break;
                        case 5:
                            Console.Clear();
                            gymController.SearchInstructor();
                            break;
                        case 0:
                            managingInstructors = false;
                            break;
                        default:
                            Console.WriteLine("Please enter a valid number.");
                            break;
                    }

                    if (managingInstructors)
                    {
                        Console.WriteLine("\nPress Enter to return to the Instructors menu...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void AddMember(GymController gymController)
        {
            try
            {
                Console.Clear();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("---- Adding a Member ----");
                Console.WriteLine("Enter Member Details:");

                // Member information
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Age: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Sex (Male/Female): ");
                string sex = Console.ReadLine();

                Console.Write("Weight (in kg): ");
                double weight = double.Parse(Console.ReadLine());
                
                Member newMember = new Member(name, age, sex, weight);
                
                gymController.DisplayExercisesForMember(newMember);
                // Ask for membership type
                Console.WriteLine("Select Membership Type:");
                Console.WriteLine("1. Monthly");
                Console.WriteLine("2. Annual");
                Console.Write("Choice: ");
                int membershipChoice = int.Parse(Console.ReadLine());

                MembershipType membershipType = MembershipType.Monthly; // Default

                switch (membershipChoice)
                {
                    case 1:
                        membershipType = MembershipType.Monthly;
                        break;
                    case 2:
                        membershipType = MembershipType.Annual;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, defaulting to Monthly.");
                        break;
                }

                // Ask for payment method
                Console.WriteLine("Select Payment Method:");
                Console.WriteLine("1. Credit Card");
                Console.WriteLine("2. Cash");
                Console.Write("Choice: ");
                string paymentMethod = Console.ReadLine();

                // Create the payment object
                Payment newPayment = new Payment(membershipType, paymentMethod);

                // Associate the payment with the member

                newMember.Payment = newPayment;
                gymController.AddMember(newMember);
                gymController.PrintReceipt(newMember, newPayment);

                Console.WriteLine();
                Console.WriteLine("Member added successfully with the selected membership and payment details!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter the correct data type for each field.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while adding member: {ex.Message}");
            }
        }

        public static void ChangeMemberStatus(GymController gymController)
        {
            Console.Write("Enter the name of the member to change status: ");
            string memberName = Console.ReadLine();

            // Find the member by name
            var member = gymController.GetMemberByName(memberName);
            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return;
            }

            // Toggle the member's status
            member.IsActive = !member.IsActive;  // If active, set to inactive and vice versa

            string status = member.IsActive ? "Active" : "Inactive";
            Console.WriteLine($"Member status for {member.Name} has been changed to {status}.");
        }


        static void AddInstructor(GymController gymController)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("---- Adding instructor ----");
                Console.WriteLine("Enter Instructor Details:");

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Age: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Sex (Male/Female): ");
                string sex = Console.ReadLine();

                Console.Write("Specialty: ");
                string specialty = Console.ReadLine();

                Instructor newInstructor = new Instructor(name, age, sex, specialty);
                gymController.AddInstructor(newInstructor);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter the correct data type for each field.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding instructor: {ex.Message}");
            }
        }
        
    }
}
