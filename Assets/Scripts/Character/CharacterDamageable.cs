using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamageable : IDamageable
{
    public event Action<DamageTakenArgs> OnDamageTaken;
    public event Action<DeathParameters> OnDeath;

    private IHealthSetter _health;
    private CharacterInfo _info;
    private UnityClient _client;
    private SceneGameplayProperties _damageManager;

    public CharacterDamageable(IHealthSetter health, CharacterInfo info, UnityClient client, SceneGameplayProperties damageManager)
    {
        _health = health;
        _info = info;
        _client = client;
        _damageManager = damageManager;
    }

    /// <summary>
    /// Deals damage and returns damage actually dealt
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public float TakeDamage(float amount)
    {
        var healthAfter = Mathf.Clamp(_health.Health - amount, 0.0f, _health.MaxHealth);
        var dmgDealt = _health.Health - healthAfter;

        if (_info.IsLocal)
        {
            if (_damageManager.NegateDamage == false)
            {
                _health.Health = healthAfter;
            }
            OnDamageTaken?.Invoke(new DamageTakenArgs(dmgDealt, _health.Health));

            if (_health.Health <= 0.0f)
            {
                DeathParameters deathParameters = new DeathParameters();
                deathParameters.characterInfo = _info;
                OnDeath?.Invoke(deathParameters);
            }
        }
        else
        {
            SendMessage(amount); //we use {amount} instead of {dmgDealt} because HP may not be up to date on the client
        }

        return dmgDealt;
    }

    private void SendMessage(float damage)
    {
        using(var writer = DarkRiftWriter.Create())
        {
            writer.Write(_info.Id);
            writer.Write(damage);

            using (var msg = Message.Create(Tags.TakeDamage, writer))
            {
                _client.SendMessage(msg, SendMode.Reliable);
            }
        }
    }
}
