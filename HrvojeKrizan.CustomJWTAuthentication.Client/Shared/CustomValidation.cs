using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace HrvojeKrizan.CustomJWTAuthentication.Client.Shared
{
    public class CustomValidation : ComponentBase, IDisposable
    {
        IDisposable? _subscriptions;

        ValidationMessageStore? _messageStore;

        [CascadingParameter]
        EditContext? CurrentEditContext { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(CustomValidation)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. " +
                    $"For example, you can use {nameof(CustomValidation)} " +
                    $"inside an {nameof(EditForm)}.");
            }

            _messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += CurrentEditContext_OnValidationRequested;
            CurrentEditContext.OnFieldChanged += CurrentEditContext_OnFieldChanged;
        }

        private void CurrentEditContext_OnFieldChanged(object? sender, FieldChangedEventArgs e)
        {
            _messageStore?.Clear(e.FieldIdentifier);
        }

        private void CurrentEditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
        {
            _messageStore?.Clear();
        }

        public void DisplayErrors(Dictionary<string, List<string>> errors)
        {
            if (CurrentEditContext != null)
            {
                foreach (var err in errors)
                {
                    _messageStore?.Add(CurrentEditContext.Field(err.Key), err.Value);
                }

                CurrentEditContext.NotifyValidationStateChanged();
            }
        }

        public void ClearErrors()
        {
            _messageStore?.Clear();
            CurrentEditContext?.NotifyValidationStateChanged();
        }

        public void Dispose()
        {
            if (CurrentEditContext != null)
            {
                CurrentEditContext.OnValidationRequested -= CurrentEditContext_OnValidationRequested;
                CurrentEditContext.OnFieldChanged -= CurrentEditContext_OnFieldChanged;
            }
        }
    }
}
