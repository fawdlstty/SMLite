package cn.fawdlstty.smlite.internface;

public interface IFunc_st<TState, TTrigger> {
    public TState call (TState state, TTrigger trigger) throws Exception;
}
