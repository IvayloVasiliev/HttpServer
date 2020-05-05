using SIS.MvcFramework.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public class ControllerState : IControllerState
    {
        public ModelStateDictionary ModelState { get; set; }

        public ControllerState()
        {
            this.Reset();
        }

        public void Initialize(Controller controller)
        {
            this.ModelState = controller.ModelState;
        }

        public void Reset()
        {
            this.ModelState = new ModelStateDictionary(); ;
        }

        public void SetState(Controller controller)
        {
            controller.ModelState = this.ModelState;
        }
    }
}
