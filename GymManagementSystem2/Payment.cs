using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem2
{
    public class Payment
    {
        public double TotalAmount { get; set; } 
        public double AmountPaid { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public MembershipType MembershipType { get; set; }
        public DateTime DueDate { get; set; } 

        public Payment(MembershipType membershipType, string paymentMethod)
        {
            MembershipType = membershipType;
            PaymentMethod = paymentMethod;
            PaymentDate = DateTime.Now;
            Amount = CalculateFee();
            DueDate = CalculateDueDate(); 
        }

        private double CalculateFee()
        {
            double fee = 0;

            switch (MembershipType)
            {
                case MembershipType.Annual:
                    fee = 3000;
                    break;
                case MembershipType.Monthly:
                    fee = 250; 
                    break;
                default:
                    throw new InvalidOperationException("Unknown membership type");
            }

            return fee;
        }

        // Calculate the due date based on membership type
        private DateTime CalculateDueDate()
        {
            DateTime dueDate = PaymentDate;

            switch (MembershipType)
            {
                case MembershipType.Annual:
                    dueDate = PaymentDate.AddYears(1);
                    break;
                case MembershipType.Monthly:
                    dueDate = PaymentDate.AddMonths(1);
                    break;
                default:
                    throw new InvalidOperationException("Unknown membership type");
            }

            return dueDate;
        }
    }

    public enum MembershipType
    {
        Annual,
        Monthly
    }
}
