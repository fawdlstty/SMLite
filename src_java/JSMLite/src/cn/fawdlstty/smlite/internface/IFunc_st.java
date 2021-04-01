package cn.fawdlstty.smlite.internface;

public interface IFunc_st<TState extends Enum, TTrigger extends Enum> {
    public TState call (TState state, TTrigger trigger) throws Exception;
}
