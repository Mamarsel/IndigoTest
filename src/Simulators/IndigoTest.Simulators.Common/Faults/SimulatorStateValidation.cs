namespace IndigoTest.Simulators.Common.Faults;

public static class SimulatorStateValidation
{
    public static IReadOnlyList<string> Validate(SimulatorState state)
    {
        var errors = new List<string>();

        if (state.DuplicateChance is < 0 or > 1)
        {
            errors.Add("DuplicateChance must be between 0 and 1.");
        }

        if (state.SilenceEnabled)
        {
            if (state.SilenceDurationMs is null or <= 0)
            {
                errors.Add("SilenceDurationMs must be greater than 0 when SilenceEnabled is true.");
            }

            if (state.SilenceAfterMessages is not null and <= 0)
            {
                errors.Add("SilenceAfterMessages must be greater than 0 when it is specified.");
            }
        }
        else if (state.SilenceDurationMs is not null || state.SilenceAfterMessages is not null)
        {
            errors.Add("SilenceDurationMs and SilenceAfterMessages are allowed only when SilenceEnabled is true.");
        }

        if (state.OutageEnabled)
        {
            if (state.OutageDurationMs is null or <= 0)
            {
                errors.Add("OutageDurationMs must be greater than 0 when OutageEnabled is true.");
            }

            if (state.OutageAfterMessages is not null and <= 0)
            {
                errors.Add("OutageAfterMessages must be greater than 0 when it is specified.");
            }
        }
        else if (state.OutageDurationMs is not null || state.OutageAfterMessages is not null)
        {
            errors.Add("OutageDurationMs and OutageAfterMessages are allowed only when OutageEnabled is true.");
        }

        if (state.DisconnectMode == DisconnectMode.AfterMessageCount)
        {
            if (state.DisconnectAfterMessages is null or <= 0)
            {
                errors.Add("DisconnectAfterMessages must be greater than 0 for AfterMessageCount mode.");
            }
        }
        else if (state.DisconnectAfterMessages is not null)
        {
            errors.Add("DisconnectAfterMessages is allowed only for AfterMessageCount mode.");
        }

        return errors;
    }
}
