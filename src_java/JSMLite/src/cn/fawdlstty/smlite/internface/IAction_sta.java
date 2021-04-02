package cn.fawdlstty.smlite.internface;

public interface IAction_sta<TState, TTrigger> {
    public void call (TState state, TTrigger trigger, Object[] args) throws Exception;
}
