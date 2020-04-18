﻿using System;

namespace OrkadWeb.Services.DTO.Expenses
{
    public class ExpenseItem : Ownable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}