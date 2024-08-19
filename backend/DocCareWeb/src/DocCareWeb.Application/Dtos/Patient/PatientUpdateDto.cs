﻿using DocCareWeb.Application.Dtos.Address;
using DocCareWeb.Domain.Enums;
using System.Text.Json.Serialization;

namespace DocCareWeb.Application.Dtos.Patient
{
    public class PatientUpdateDto : PatientBaseDto
    {
        public PatientUpdateDto(string name, string? cpf, string? rg, Gender gender, DateTime birthDate, string? email, string? phone, string? cellPhone, string? notes, int healthPlanId, AddressUpdateDto address)
        : base(name, cpf, rg, gender, birthDate, email, phone, cellPhone, notes, healthPlanId)
        {
            Address = address;
        }

        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(100)]
        public required AddressUpdateDto Address { get; set; }

    }
}
