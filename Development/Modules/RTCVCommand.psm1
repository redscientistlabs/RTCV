class RTCVCommand {
    [string]$PrintFriendlyName
    [string]$Command

    RTCVCommand(
        [string]$n,
        [string]$c
    ){
        $this.PrintFriendlyName = $n
        $this.Command = $c
    }
}
