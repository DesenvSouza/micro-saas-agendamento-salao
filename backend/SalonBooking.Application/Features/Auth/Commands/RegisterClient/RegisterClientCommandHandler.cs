using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Auth.Commands.RegisterClient;

public class RegisterClientCommandHandler : IRequestHandler<RegisterClientCommand, RegisterClientResponse>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterClientCommandHandler(
        IClientRepository clientRepository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _clientRepository = clientRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterClientResponse> Handle(
        RegisterClientCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository.ExistsWithEmailAsync(request.Email, cancellationToken);
        if (emailExists)
            throw new BusinessRuleViolationException("Este e-mail já está cadastrado.");

        var passwordHash = _passwordHasher.Hash(request.Password);
        var client = Client.Create(request.Email, passwordHash, request.FullName, request.Phone);

        await _clientRepository.AddAsync(client, cancellationToken);

        return new RegisterClientResponse(
            client.Id,
            client.Email,
            client.FullName,
            "Cadastro realizado com sucesso! Verifique seu e-mail para ativar a conta.");
    }
}
