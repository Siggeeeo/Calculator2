namespace Calculator2
{

    public partial class MainPage : ContentPage
    {
        private string currentEntry = "";       // Håller reda på aktuell inmatning
        private double? firstNumber = null;     // Första numret i beräkningen
        private string pendingOperator = null;  // Operatör som väntar på andra numret

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;

            switch (buttonText)
            {
                case "C":
                    ResetCalculator();
                    break;

                case "=":
                    CalculateResult();
                    break;

                case "+":
                case "-":
                case "×":
                case "÷":
                    HandleOperator(buttonText);
                    break;

                case "±":
                    ChangeSign();
                    break;

                case "%":
                    CalculatePercentage();
                    break;

                case ".":
                    AddDecimalPoint();
                    break;

                default:
                    AddNumber(buttonText);
                    break;
            }

            UpdateDisplay();
        }

        private void ResetCalculator()
        {
            currentEntry = "";
            firstNumber = null;
            pendingOperator = null;
        }

        private void AddNumber(string number)
        {
            currentEntry += number;
        }

        private void AddDecimalPoint()
        {
            if (!currentEntry.Contains("."))
            {
                currentEntry += string.IsNullOrEmpty(currentEntry) ? "0." : ".";
            }
        }

        private void HandleOperator(string newOperator)
        {
            if (!string.IsNullOrEmpty(currentEntry))
            {
                if (firstNumber == null)
                {
                    firstNumber = double.Parse(currentEntry);
                }
                else if (pendingOperator != null)
                {
                    firstNumber = Calculate(firstNumber.Value, pendingOperator, double.Parse(currentEntry));
                }
                pendingOperator = newOperator;
                currentEntry = "";
            }   
        }

        private void CalculateResult()
        {
            if (firstNumber != null && pendingOperator != null && !string.IsNullOrEmpty(currentEntry))
            {
                double secondNumber = double.Parse(currentEntry);
                double result = Calculate(firstNumber.Value, pendingOperator, secondNumber);
                currentEntry = result.ToString();
                firstNumber = null;
                pendingOperator = null;
            }
        }

        private double Calculate(double a, string op, double b)
        {
            switch (op)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "×": return a * b;
                case "÷":
                    if (b == 0)
                    {
                        DisplayAlert("Fel", "Division med noll!", "OK");
                        ResetCalculator();
                        return 0;
                    }
                    return a / b;
                default: return 0;
            }
        }

        private void ChangeSign()
        {
            if (!string.IsNullOrEmpty(currentEntry))
            {
                double number = double.Parse(currentEntry);
                number *= -1;
                currentEntry = number.ToString();
            }
        }

        private void CalculatePercentage()
        {
            if (!string.IsNullOrEmpty(currentEntry))
            {
                double number = double.Parse(currentEntry);
                number /= 100;
                currentEntry = number.ToString();
            }
        }

        private void UpdateDisplay()
        {
            DisplayLabel.Text = string.IsNullOrEmpty(currentEntry) ? "0" : currentEntry;
        }
    }
}