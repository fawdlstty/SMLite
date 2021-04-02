package cn.fawdlstty.smlite.internface;

public interface IAction_s<TState> {
    public void call (TState state) throws Exception;
}
