package cn.fawdlstty.smlite.internface;

public interface IFunc_sta<TState extends Enum, TTrigger extends Enum> {
    public TState call (TState state, TTrigger trigger, Object[] args) throws Exception;
}
