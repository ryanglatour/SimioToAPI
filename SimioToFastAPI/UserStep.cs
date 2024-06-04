using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimioAPI;
using SimioAPI.Extensions;

namespace SimioToFastAPI
{
    internal class UserStepDefinition : IStepDefinition
    {
        #region IStepDefinition Members

        /// <summary>
        /// Property returning the full name for this type of step. The name should contain no spaces.
        /// </summary>
        public string Name
        {
            get { return "SimioToAPIStep"; }
        }

        /// <summary>
        /// Property returning a short description of what the step does.
        /// </summary>
        public string Description
        {
            get { return "Calls API endpoint and assigns json response to associated object state."; }
        }

        /// <summary>
        /// Property returning an icon to display for the step in the UI.
        /// </summary>
        public System.Drawing.Image Icon
        {
            get { return null; }
        }

        /// <summary>
        /// Property returning a unique static GUID for the step.
        /// </summary>
        public Guid UniqueID
        {
            get { return MY_ID; }
        }
        static readonly Guid MY_ID = new Guid("{1dc5e58e-14ba-42e6-bd89-ecb37f4a9d4a}");

        /// <summary>
        /// Property returning the number of exits out of the step. Can return either 1 or 2.
        /// </summary>
        public int NumberOfExits
        {
            get { return 1; }
        }

        /// <summary>
        /// Method called that defines the property schema for the step.
        /// </summary>
        public void DefineSchema(IPropertyDefinitions schema)
        {
            // path property
            IPropertyDefinition pd;
            pd = schema.AddExpressionProperty("Path", "");
            pd.DisplayName = "Path";
            pd.Description = "API Path ('http://127.0.0.1:8000/api/...').";
            pd.Required = true;

            // JSON State Name
            pd = schema.AddExpressionProperty("EntityStateName", "");
            pd.DisplayName = "Entity State Name";
            pd.Description = "String containing name of entity's state.";
            pd.Required = true;
        }

        /// <summary>
        /// Method called to create a new instance of this step type to place in a process.
        /// Returns an instance of the class implementing the IStep interface.
        /// </summary>
        public IStep CreateStep(IPropertyReaders properties)
        {
            return new UserStep(properties);
        }

        #endregion
    }

    internal class UserStep : IStep
    {
        IPropertyReaders _properties;

        public UserStep(IPropertyReaders properties)
        {
            _properties = properties;
        }

        #region IStep Members

        /// <summary>
        /// Method called when a process token executes the step.
        /// </summary>
        public ExitType Execute(IStepExecutionContext context)
        {
            // Fetch path and state strings
            IPropertyReader pathNameProp = _properties.GetProperty("Path") as IPropertyReader;
            string path = pathNameProp.GetStringValue(context);
            path = path.Trim('"');

            IPropertyReader stateNameProp = _properties.GetProperty("EntityStateName") as IPropertyReader;
            string stateName = stateNameProp.GetStringValue(context);
            stateName = stateName.Trim('"');


            // Fetch the associated object (entity)
            IElementData associatedObject = context.AssociatedObject;
            
            IStates associatedStates = associatedObject.States;

            IStringState jsonState = associatedStates[stateName] as IStringState;


            // Call API
            API api = new API();
            string jsonResponse = api.CallAPI(path);

            // Set state to json string
            jsonState.Value = jsonResponse;

            return ExitType.FirstExit;
        }

        #endregion
    }
}
