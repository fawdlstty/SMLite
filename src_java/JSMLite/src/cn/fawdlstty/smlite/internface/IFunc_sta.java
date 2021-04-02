package cn.fawdlstty.smlite.internface;

public interface IFunc_sta<TState, TTrigger> {
    public TState call (TState state, TTrigger trigger, Object[] args) throws Exception;
}
