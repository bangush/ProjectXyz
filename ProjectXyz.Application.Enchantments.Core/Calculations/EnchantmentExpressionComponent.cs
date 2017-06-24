using ProjectXyz.Api.Enchantments.Calculations;

namespace ProjectXyz.Application.Enchantments.Core.Calculations
{
    public sealed class EnchantmentExpressionComponent : IEnchantmentExpressionComponent
    {
        #region Constructors
        public EnchantmentExpressionComponent(
            ICalculationPriority calculationPriority,
            string expression)
        {
            CalculationPriority = calculationPriority;
            Expression = expression;
        }
        #endregion

        #region Properties
        public string Expression { get; }

        public ICalculationPriority CalculationPriority { get; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"'{GetType()}'\r\n\tCalculation Priority: {CalculationPriority}\r\n\tExpression: {Expression}";
        }
        #endregion
    }
}