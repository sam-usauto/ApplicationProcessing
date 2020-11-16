using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Application
{
    public class ApplicationFlowStep
    {
        public string FlowName { get; set; }
        public int Id { get; set; }
        public string ClientApplicationId { get; set; }
        public string ApplicationFlowStepId { get; set; }
        public bool IsCompleted { get; set; }
        public int StepOrder { get; set; }
        public string ExecuteUrl { get; set; }
    }
}
