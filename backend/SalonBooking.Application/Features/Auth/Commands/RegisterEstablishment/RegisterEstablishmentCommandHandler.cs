using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Auth.Commands.RegisterEstablishment;

public class RegisterEstablishmentCommandHandler
    : IRequestHandler<RegisterEstablishmentCommand, RegisterEstablishmentResponse>
{
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterEstablishmentCommandHandler(
        IEstablishmentRepository establishmentRepository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _establishmentRepository = establishmentRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterEstablishmentResponse> Handle(
        RegisterEstablishmentCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository.ExistsWithEmailAsync(request.Email, cancellationToken);
        if (emailExists)
            throw new BusinessRuleViolationException("Este e-mail já está cadastrado.");

        var passwordHash = _passwordHasher.Hash(request.Password);
        var establishment = Establishment.Create(
            request.Email,
            passwordHash,
            request.TradeName,
            request.Street,
            request.City,
            request.State,
            request.ZipCode,
            request.Latitude,
            request.Longitude);

        await _establishmentRepository.AddAsync(establishment, cancellationToken);

        return new RegisterEstablishmentResponse(
            establishment.Id,
            establishment.Email,
            establishment.TradeName,
            "Estabelecimento cadastrado com sucesso! Aguarde a aprovação para começar a receber agendamentos.");
    }
}
