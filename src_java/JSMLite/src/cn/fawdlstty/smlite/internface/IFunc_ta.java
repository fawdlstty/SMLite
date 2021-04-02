package cn.fawdlstty.smlite.internface;

public interface IFunc_ta<TState, TTrigger> {
    public TState call (TTrigger trigger, Object[] args) throws Exception;
}
