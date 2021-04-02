package cn.fawdlstty.smlite.internface;

public interface IAction_sa<TState> {
    public void call (TState state, Object[] args) throws Exception;
}
