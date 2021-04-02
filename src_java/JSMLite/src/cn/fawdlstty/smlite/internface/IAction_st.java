package cn.fawdlstty.smlite.internface;

public interface IAction_st<TState, TTrigger> {
    public void call (TState state, TTrigger trigger) throws Exception;
}
