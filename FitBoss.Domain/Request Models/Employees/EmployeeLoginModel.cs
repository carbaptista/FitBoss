﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Request_Models.Employees;
public class EmployeeLoginModel
{
    /// <summary>
    /// Username - Usuário
    /// </summary>
    /// <example>carhab</example>
    [Required(ErrorMessage = "Username is required")]
    [DefaultValue("carhab")]
    public string UserName { get; set; } = "";

    /// <summary>
    /// Password - Senha
    /// </summary>
    /// <example>password</example>
    [DataType(DataType.Password)]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}
