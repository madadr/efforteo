﻿using System;
using Efforteo.Common.Events;

namespace Efforteo.Common.Commands
{
    public class CreateActivity : IAuthenticatedCommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long Time { get; set; }
        public float Distance { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}