/*
 * Project Name: Langham Hotel Management System
 * Author Name: Harman
 * Date: 30/10/2026
 * Application Purpose: Manage hotel rooms and customer allocations.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assessment2Task2
{
    // Custom Class - Room
    public class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; }
    }

    // Custom Class - Customer
    public class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }

    // Custom Class - RoomAllocation
    public class RoomAllocation
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; }
    }

    // Custom Main Class - Program
    class Program
    {
        public static Room[] listOfRooms = new Room[100];
        public static RoomAllocation[] listOfRoomAllocations =
            new RoomAllocation[100];

        public static int roomCount = 0;
        public static int allocationCount = 0;
        public static string filePath;

        // Main Function
        static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);

            filePath = Path.Combine(folderPath, "HotelManagement.txt");

            char ans = 'y';

            do
            {
                Console.Clear();

                Console.WriteLine("****************************************");
                Console.WriteLine("    LANGHAM HOTEL MANAGEMENT SYSTEM");
                Console.WriteLine("****************************************");
                Console.WriteLine("1. Add Rooms");
                Console.WriteLine("2. Display Rooms");
                Console.WriteLine("3. Allocate Rooms");
                Console.WriteLine("4. De-Allocate Rooms");
                Console.WriteLine("5. Display Room Allocation Details");
                Console.WriteLine("6. Billing");
                Console.WriteLine("7. Save Room Allocations To a File");
                Console.WriteLine("8. Show Room Allocations From a File");
                Console.WriteLine("9. Exit");
                Console.WriteLine("0. Backup");
                Console.WriteLine("10. Test Unauthorized Access ");
                Console.WriteLine("****************************************");

                Console.Write("Enter Your Choice Number Here: ");

                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid choice!");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddRooms();
                        break;

                    case 2:
                        DisplayRooms();
                        break;

                    case 3:
                        AllocateRooms();
                        break;

                    case 4:
                        DeAllocateRooms();
                        break;

                    case 5:
                        DisplayAllocations();
                        break;

                    case 6:
                        Console.WriteLine(
                            "Billing Feature is Under Construction and will be added soon!");
                        break;

                    case 7:
                        SaveRoomAllocationsToFile();
                        break;

                    case 8:
                        ShowRoomAllocationsFromFile();
                        break;

                    case 9:
                        Console.WriteLine("Thank you for using Langham Hotel!");
                        return;

                    case 0:
                        BackupFile();
                        break;

                    case 10:
                        TestUnauthorizedAccess();
                        break;

                    default:
                        Console.WriteLine("Invalid menu choice!");
                        break;
                }

                Console.Write("\nWould You Like To Continue (Y/N): ");
                ans = Convert.ToChar(Console.ReadLine().Trim().ToUpper()[0]);

            } while (ans == 'y' || ans == 'Y');
        }

        // 1. Add Rooms
        static void AddRooms()
        {
            Console.Write("Enter number of rooms to add: ");
            int number;

            if (!int.TryParse(Console.ReadLine(), out number) ||
                number <= 0 || roomCount + number > listOfRooms.Length)
            {
                Console.WriteLine("Invalid number of rooms!");
                return;
            }

            for (int i = 0; i < number; i++)
            {
                Console.Write("Enter Room Number: ");
                int roomNo;

                if (!int.TryParse(Console.ReadLine(), out roomNo) ||
                    roomNo <= 0 ||
                    listOfRooms.Take(roomCount).Any(r => r.RoomNo == roomNo))
                {
                    Console.WriteLine("Invalid or duplicate room number!");
                    i--;
                    continue;
                }

                listOfRooms[roomCount] = new Room
                {
                    RoomNo = roomNo,
                    IsAllocated = false
                };

                roomCount++;
                Console.WriteLine("Room added successfully!");
            }
        }

        // 2. Display Rooms
        static void DisplayRooms()
        {
            Console.WriteLine("\nROOM DETAILS");

            if (roomCount == 0)
            {
                Console.WriteLine("No rooms available.");
                return;
            }

            foreach (Room room in listOfRooms.Take(roomCount))
            {
                Console.WriteLine("Room Number: " + room.RoomNo);
                Console.WriteLine("Status: " +
                    (room.IsAllocated ? "Allocated" : "Available"));
                Console.WriteLine("----------------------");
            }
        }

        // 3. Allocate Rooms
        static void AllocateRooms()
        {
            Console.Write("Enter Room Number: ");
            int roomNo;

            if (!int.TryParse(Console.ReadLine(), out roomNo))
            {
                Console.WriteLine("Invalid room number!");
                return;
            }

            Room room = listOfRooms.Take(roomCount)
                .FirstOrDefault(r => r.RoomNo == roomNo);

            if (room == null)
            {
                Console.WriteLine("Room not found!");
                return;
            }

            if (room.IsAllocated)
            {
                Console.WriteLine("Room is already allocated!");
                return;
            }

            if (allocationCount >= listOfRoomAllocations.Length)
            {
                Console.WriteLine("Allocation limit reached!");
                return;
            }

            Console.Write("Enter Customer Number: ");
            int customerNo;

            if (!int.TryParse(Console.ReadLine(), out customerNo) ||
                customerNo <= 0)
            {
                Console.WriteLine("Invalid customer number!");
                return;
            }

            Console.Write("Enter Customer Name: ");
            string customerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(customerName))
            {
                Console.WriteLine("Customer name cannot be empty!");
                return;
            }

            Customer customer = new Customer
            {
                CustomerNo = customerNo,
                CustomerName = customerName
            };

            listOfRoomAllocations[allocationCount] =
                new RoomAllocation
                {
                    AllocatedRoomNo = roomNo,
                    AllocatedCustomer = customer
                };

            allocationCount++;
            room.IsAllocated = true;

            Console.WriteLine("Room allocated successfully!");
        }

        // 4. De-Allocate Rooms
        static void DeAllocateRooms()
        {
            Console.Write("Enter Room Number to De-Allocate: ");
            int roomNo;

            if (!int.TryParse(Console.ReadLine(), out roomNo))
            {
                Console.WriteLine("Invalid room number!");
                return;
            }

            int index = Array.FindIndex(
                listOfRoomAllocations,
                0,
                allocationCount,
                a => a.AllocatedRoomNo == roomNo);

            if (index == -1)
            {
                Console.WriteLine("Room allocation not found!");
                return;
            }

            listOfRoomAllocations[index] = null;

            Room room = listOfRooms.Take(roomCount)
                .FirstOrDefault(r => r.RoomNo == roomNo);

            if (room != null)
            {
                room.IsAllocated = false;
            }

            Console.WriteLine("Room de-allocated successfully!");
        }

        // 5. Display Room Allocation Details
        static void DisplayAllocations()
        {
            Console.WriteLine("\nROOM ALLOCATION DETAILS");

            bool found = false;

            foreach (RoomAllocation allocation in
                listOfRoomAllocations.Take(allocationCount))
            {
                if (allocation == null)
                    continue;

                found = true;

                Console.WriteLine("Room Number: " +
                    allocation.AllocatedRoomNo);

                Console.WriteLine("Customer Number: " +
                    allocation.AllocatedCustomer.CustomerNo);

                Console.WriteLine("Customer Name: " +
                    allocation.AllocatedCustomer.CustomerName);

                Console.WriteLine("----------------------");
            }

            if (!found)
                Console.WriteLine("No room allocations found.");
        }

        // 7. Save Room Allocations To File
        static void SaveRoomAllocationsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (RoomAllocation allocation in
                        listOfRoomAllocations.Take(allocationCount))
                    {
                        if (allocation == null)
                            continue;

                        writer.WriteLine(
                            allocation.AllocatedRoomNo + "," +
                            allocation.AllocatedCustomer.CustomerNo + "," +
                            allocation.AllocatedCustomer.CustomerName);
                    }
                }

                Console.WriteLine("Room allocations saved successfully!");
                Console.WriteLine("File Location: " + filePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine("File Error: " + ex.Message);
            }
        }

        // 8. Show Room Allocations From File
        static void ShowRoomAllocationsFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File does not exist!");
                    return;
                }

                Console.WriteLine("\nSAVED ROOM ALLOCATIONS");

                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length == 0)
                {
                    Console.WriteLine("File is empty.");
                    return;
                }

                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("File Error: " + ex.Message);
            }
        }

        // - Demonstrate UnauthorizedAccessException
        static void TestUnauthorizedAccess()
        {
            try
            {
                // Set the file as read-only before trying to write to it.
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "Permission test file.");
                }

                File.SetAttributes(filePath, FileAttributes.ReadOnly);

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Testing file permissions.");
                }

                Console.WriteLine("File written successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Unauthorized Access Error: " + ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("File Error: " + ex.Message);
            }
            finally
            {
                // Restore normal file attributes after the test.
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                }

                Console.WriteLine("File access test completed.");
            }
        }

        // 0. Backup File
        static void BackupFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("No file available to backup!");
                    return;
                }

                string backupPath = Path.Combine(
                    Path.GetDirectoryName(filePath),
                    "lhms_studentid_backup.txt");

                File.Copy(filePath, backupPath, true);

                Console.WriteLine("Backup created successfully!");
                Console.WriteLine("Backup Location: " + backupPath);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Backup Error: " + ex.Message);
            }
        }
    }
}
