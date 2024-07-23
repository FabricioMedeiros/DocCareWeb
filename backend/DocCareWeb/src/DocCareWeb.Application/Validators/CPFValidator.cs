namespace DocCareWeb.Application.Validators
{
    public static class CPFValidator
    {
        public static bool IsValid(string? cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais
            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            // Calcula o primeiro dígito verificador
            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];
            var resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            if (resto != int.Parse(cpf[9].ToString()))
                return false;

            // Calcula o segundo dígito verificador
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            if (resto != int.Parse(cpf[10].ToString()))
                return false;

            return true;
        }
    }
}
