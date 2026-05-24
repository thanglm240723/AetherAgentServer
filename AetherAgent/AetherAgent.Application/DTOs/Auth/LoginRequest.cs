namespace AetherAgent.Application.DTOs.Auth;

// EARS[WHEN user submits credentials THE SYSTEM SHALL validate against DB and return tokens]
public sealed record LoginRequest(string Username, string Password);
