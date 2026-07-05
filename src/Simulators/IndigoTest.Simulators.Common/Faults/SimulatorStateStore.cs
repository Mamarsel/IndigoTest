namespace IndigoTest.Simulators.Common.Faults;

public sealed class SimulatorStateStore
{
    private readonly object sync = new();

    private SimulatorState state = new();

    private int messagesBeforeDisconnect;

    private int messagesBeforeSilence;

    private int messagesBeforeOutage;

    private DateTimeOffset? unavailableUntilUtc;

    public SimulatorState Get()
    {
        lock (sync)
        {
            return Copy(state);
        }
    }

    public SimulatorState Update(SimulatorState nextState)
    {
        var errors = SimulatorStateValidation.Validate(nextState);
        if (errors.Count > 0)
        {
            throw new InvalidOperationException(string.Join(" ", errors));
        }

        var normalizedState = Normalize(nextState);

        lock (sync)
        {
            state = normalizedState;
            messagesBeforeDisconnect = normalizedState.DisconnectAfterMessages ?? 0;
            messagesBeforeSilence = normalizedState.SilenceAfterMessages ?? 0;
            messagesBeforeOutage = normalizedState.OutageAfterMessages ?? 0;

            if (normalizedState.CancelOutageRequested)
            {
                unavailableUntilUtc = null;
                state = state with
                {
                    CancelOutageRequested = false,
                };
            }

            if (normalizedState.OutageEnabled && normalizedState.OutageAfterMessages is null)
            {
                ActivateOutage(normalizedState.OutageDurationMs!.Value);
            }

            return Copy(state);
        }
    }

    public bool TryGetUnavailableUntilUtc(out DateTimeOffset? unavailableUntil)
    {
        lock (sync)
        {
            if (unavailableUntilUtc is null)
            {
                unavailableUntil = null;

                return false;
            }

            if (unavailableUntilUtc <= DateTimeOffset.UtcNow)
            {
                unavailableUntilUtc = null;
                unavailableUntil = null;

                return false;
            }

            unavailableUntil = unavailableUntilUtc;

            return true;
        }
    }

    public bool ShouldSendDuplicate()
    {
        lock (sync)
        {
            if (!state.DuplicatesEnabled)
            {
                return false;
            }

            return Random.Shared.NextDouble() < state.DuplicateChance;
        }
    }

    public bool ShouldDisconnectAfterMessage()
    {
        lock (sync)
        {
            switch (state.DisconnectMode)
            {
                case DisconnectMode.None:
                    return false;
                case DisconnectMode.AfterNextMessage:
                    state = state with
                    {
                        DisconnectMode = DisconnectMode.None,
                    };

                    return true;
                case DisconnectMode.AfterMessageCount:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            messagesBeforeDisconnect--;

            if (messagesBeforeDisconnect > 0)
            {
                return false;
            }

            state = state with
            {
                DisconnectMode = DisconnectMode.None,
                DisconnectAfterMessages = null,
            };

            messagesBeforeDisconnect = 0;

            return true;
        }
    }

    public TimeSpan? GetSilenceDelayAfterMessage()
    {
        lock (sync)
        {
            if (!state.SilenceEnabled || state.SilenceDurationMs is null)
            {
                return null;
            }

            if (state.SilenceAfterMessages is null)
            {
                var silenceDuration = TimeSpan.FromMilliseconds(state.SilenceDurationMs.Value);

                ResetSilenceState();

                return silenceDuration;
            }

            messagesBeforeSilence--;

            if (messagesBeforeSilence > 0)
            {
                return null;
            }

            var duration = TimeSpan.FromMilliseconds(state.SilenceDurationMs.Value);

            ResetSilenceState();

            return duration;
        }
    }

    public bool ShouldStartOutageAfterMessage()
    {
        lock (sync)
        {
            if (!state.OutageEnabled || state.OutageDurationMs is null)
            {
                return false;
            }

            if (state.OutageAfterMessages is null)
            {
                ActivateOutage(state.OutageDurationMs.Value);

                return true;
            }

            messagesBeforeOutage--;

            if (messagesBeforeOutage > 0)
            {
                return false;
            }

            ActivateOutage(state.OutageDurationMs.Value);

            return true;
        }
    }

    private static SimulatorState Normalize(SimulatorState state)
    {
        if (!state.SilenceEnabled)
        {
            state = state with
            {
                SilenceDurationMs = null,
                SilenceAfterMessages = null,
            };
        }

        if (!state.OutageEnabled)
        {
            state = state with
            {
                OutageDurationMs = null,
                OutageAfterMessages = null,
            };
        }

        if (state.DisconnectMode != DisconnectMode.AfterMessageCount)
        {
            return state with
            {
                DisconnectAfterMessages = null,
            };
        }

        return state;
    }

    private static SimulatorState Copy(SimulatorState state)
    {
        return new SimulatorState
        {
            DuplicatesEnabled = state.DuplicatesEnabled,
            DuplicateChance = state.DuplicateChance,
            SilenceEnabled = state.SilenceEnabled,
            SilenceDurationMs = state.SilenceDurationMs,
            SilenceAfterMessages = state.SilenceAfterMessages,
            OutageEnabled = state.OutageEnabled,
            OutageDurationMs = state.OutageDurationMs,
            OutageAfterMessages = state.OutageAfterMessages,
            CancelOutageRequested = false,
            DisconnectMode = state.DisconnectMode,
            DisconnectAfterMessages = state.DisconnectAfterMessages,
        };
    }

    private void ResetSilenceState()
    {
        state = state with
        {
            SilenceEnabled = false,
            SilenceDurationMs = null,
            SilenceAfterMessages = null,
        };

        messagesBeforeSilence = 0;
    }

    private void ActivateOutage(int durationMs)
    {
        unavailableUntilUtc = DateTimeOffset.UtcNow.AddMilliseconds(durationMs);
        state = state with
        {
            OutageEnabled = false,
            OutageDurationMs = null,
            OutageAfterMessages = null,
        };

        messagesBeforeOutage = 0;
    }
}
