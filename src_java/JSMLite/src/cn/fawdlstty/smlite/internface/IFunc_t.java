package cn.fawdlstty.smlite.internface;

public interface IFunc_t<TState, TTrigger> {
    public TState call (TTrigger trigger) throws Exception;
}
